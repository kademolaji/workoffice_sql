using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Mappings;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;
using WorkOffice.Services.Email;
using WorkOffice.Services.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly DataContext _context;
        private readonly IAuditTrailService _auditTrail;
        private readonly IEmailJetService _emailService;

        public UserAccountService(DataContext context, IAuditTrailService auditTrail, IEmailJetService emailService)
        {
            _context = context;
            _auditTrail = auditTrail;
            _emailService = emailService;
        }

        public void ReactivateAccount(UserAccount user)
        {
            _context.UserAccounts.Update(user);
            _context.SaveChanges();
        }


        public async Task<ApiResponse<GetResponse<AuthenticationResponse>>> Login(string userName, string password, string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "User Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(password))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Password is required." }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<GetResponse<AuthenticationResponse>>();

                var user = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Email == userName && x.Disabled == false);

                if (user == null)
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = "Invalid login credentials" }, IsSuccess = false };
                }

                //if (!user.Verified.HasValue)
                //{
                //    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = "Your account is yet to be verified. Check your email to verify your account." }, IsSuccess = false };
                //}

                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = "Invalid login credentials" }, IsSuccess = false };
                }

                // authentication successful so generate refresh tokens
                var refreshToken = generateRefreshToken(ipAddress);
                user.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from account
                removeOldRefreshTokens(user);

                // save changes to db
                _context.Update(user);
                _context.SaveChanges();
                var result = user.ToModel<AuthenticationResponse>();
                result.RefreshToken = refreshToken.Token;
                var response = new GetResponse<AuthenticationResponse>()
                {
                    Status = true,
                    Entity = result,
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<AuthenticationResponse>>> RefreshToken(string token, string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Token is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<AuthenticationResponse>>();

                var (refreshToken, user) = getRefreshToken(token);

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress);
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                user.RefreshTokens.Add(newRefreshToken);

                removeOldRefreshTokens(user);

                _context.Update(user);
                await _context.SaveChangesAsync();

                var result = user.ToModel<AuthenticationResponse>();
                result.RefreshToken = refreshToken.Token;
                var response = new GetResponse<AuthenticationResponse>()
                {
                    Status = true,
                    Entity = result,
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }



        public async Task<ApiResponse<GetResponse<AuthenticationResponse>>> Register(RegisterRequest model, string password, string origin, string ipAddress)
        {
            try
            {

                if (string.IsNullOrEmpty(model.FirstName))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "First Name is required." }, IsSuccess = false };

                }
                if (string.IsNullOrEmpty(model.LastName))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Last Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Email is required." }, IsSuccess = false };
                }
                if (!IsValidEmailAddress(model.Email))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Email is not valid." }, IsSuccess = false };
                }
                if (await EmailExists(model.Email))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = "Email already exist." }, IsSuccess = false };
                }

                if (!ValidatePassword(model.Password))
                {
                    return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = @"Your password most contain, at least one lower case letter,
                            one upper case letter, special character, one number, and not less than 8 characters length" }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<AuthenticationResponse>>();
                bool result = false;
                string refreshTokenData = null;
                UserAccount entity = null;
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(password, out passwordHash, out passwordSalt);
                        entity = new UserAccount
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Country = model.Country,
                            RoleId = (int)model.UserRole,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            LockoutEnabled = true,
                            AccessFailedCount = 0,
                            VerificationToken = randomTokenString(),
                            AcceptTerms = model.AcceptTerms,
                            Disabled = false
                        };
                        _context.UserAccounts.Add(entity);

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var user = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Email == entity.Email);
                            // authentication successful so generate refresh tokens
                            var refreshToken = generateRefreshToken(ipAddress);
                            refreshTokenData = refreshToken.Token;
                            user.RefreshTokens.Add(refreshToken);
                            // remove old refresh tokens from account
                            removeOldRefreshTokens(entity);
                            // save changes to db
                            _context.Update(entity);
                            _context.SaveChanges();
                            try
                            {
                                await sendVerificationEmail(entity, origin);
                            }
                            catch (Exception)
                            {
                            }
                            var details = $"Created new User: Name = {model.FirstName} {model.LastName}, Email = {model.Email}, Country = {model.Country}, Role = {model.UserRole}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Create");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse> { Status = false, Entity = null, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var data = entity.ToModel<AuthenticationResponse>();
                data.RefreshToken = refreshTokenData;
                var response = new GetResponse<AuthenticationResponse>()
                {
                    Status = true,
                    Entity = data,
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> UpdateUser(UpdateUserRequest model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.FirstName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "First Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.LastName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Last Name is required." }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                UserAccount entity = null;
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.UserId > 0)
                        {
                            entity = _context.UserAccounts.Find(model.UserId);
                            if (entity != null)
                            {
                                entity.FirstName = model.LastName;
                                entity.LastName = model.FirstName;
                                entity.Country = model.Country;
                                entity.Biography = model.Biography;
                                entity.LockoutEnabled = true;
                                entity.AccessFailedCount = 0;
                            }
                        }

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Update  User: Name = {model.FirstName} {model.LastName}, Biography = {model.Biography}, Country = {model.Country}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.UserId,
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> CreateAdminUser(CreateAdminUserModel model, string origin)
        {
            try
            {

                if (string.IsNullOrEmpty(model.FirstName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "First Name is required." }, IsSuccess = false };

                }
                if (string.IsNullOrEmpty(model.LastName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Last Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Email is required." }, IsSuccess = false };
                }
                if (!IsValidEmailAddress(model.Email))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Email is not valid." }, IsSuccess = false };
                }
                if (await EmailExists(model.Email))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Email already exist." }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                UserAccount entity = null;
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash($"{model.FirstName}{model.LastName}", out passwordHash, out passwordSalt);

                        entity = new UserAccount
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Country = model.Country,
                            RoleId = (int)RolesEnum.Admin,
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            LockoutEnabled = true,
                            AccessFailedCount = 0,
                            VerificationToken = randomTokenString(),
                            AcceptTerms = true,
                            Disabled = false
                        };
                        _context.UserAccounts.Add(entity);
                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var user = _context.UserAccounts.SingleOrDefault(x => x.Email == model.Email);
                            user.ResetToken = randomTokenString();
                            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                            // send email
                            try
                            {
                                await SendPasswordResetEmail(entity, origin);
                            }
                            catch (Exception)
                            {
                            }

                            var details = $"Create  User: Name = {model.FirstName} {model.LastName}, Email: {model.Email} Country = {model.Country}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Create");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.UserId,
                    Message = "Record created successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> ChangePassword(ChangePasswordModel model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Current Password is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "New Password is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Email))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Email  is required." }, IsSuccess = false };
                }
                var user = _context.UserAccounts.Where(x => x.Email.ToLower().Trim() == model.Email.ToLower().Trim()).FirstOrDefault();
                if (user == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "User does not exist." }, IsSuccess = false };
                }

                if (!VerifyPasswordHash(model.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Invalid current password" }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(model.NewPassword, out passwordHash, out passwordSalt);

                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Change Password: Name = {user.FirstName} {user.LastName}, Email = {user.Email}, Role = {user.RoleId}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = user.UserId,
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        public async Task<ApiResponse<CreateResponse>> VerifyEmail(string token, string origin)
        {
            try
            {

                var user = _context.UserAccounts.SingleOrDefault(x => x.VerificationToken == token);
                if (user == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Verification failed" }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        user.Verified = DateTime.UtcNow;
                        user.VerificationToken = null;

                        _context.UserAccounts.Update(user);

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            // Send Email
                            await sendVerificationSuccessfulEmail(user, origin);

                            var details = $"Verified Account: Name = {user.FirstName} {user.LastName}, Email = {user.Email}, Role = {user.RoleId}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = user.UserId,
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            try
            {
                var user = _context.UserAccounts.SingleOrDefault(x => x.Email == model.Email && !x.Disabled);

                if (user == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "The email you entered was not found. Please try again." }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // create reset token that expires after 1 day
                        user.ResetToken = randomTokenString();
                        user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

                        _context.UserAccounts.Update(user);

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            // send email
                            await SendPasswordResetEmail(user, origin);
                            var details = $"Forget Password: Name = {user.FirstName} {user.LastName}, Email = {user.Email}, Role = {user.RoleId}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = user.UserId,
                    Message = "Please check your email for password reset instructions"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> ResetPassword(ResetPasswordRequest model)
        {
            try
            {

                var user = _context.UserAccounts.SingleOrDefault(x => x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);
                if (user == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Invalid Token." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                        // update password and remove reset token
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        user.PasswordReset = DateTime.UtcNow;
                        user.ResetToken = null;
                        user.ResetTokenExpires = null;

                        _context.UserAccounts.Update(user);

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Reset Password: Name = {user.FirstName} {user.LastName}, Email = {user.Email}, Role = {user.RoleId}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = user.UserId,
                    Message = "Password reset successful, you can now login"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        public async Task<ApiResponse<CreateResponse>> ValidateResetToken(ValidateResetTokenRequest model)
        {
            try
            {
                var user = await _context.UserAccounts.SingleOrDefaultAsync(x => x.ResetToken == model.Token && x.ResetTokenExpires > DateTime.UtcNow);

                var apiResponse = new ApiResponse<CreateResponse>();
                var response = new CreateResponse
                {
                    Status = user != null,
                    Id = "",
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<UserAccountResponse>>> GetUserById(Guid userId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<UserAccountResponse>>();
                var user = await _context.UserAccounts.Where(x => x.UserId == userId).Select(u => new UserAccountResponse
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProfilePicture = u.ProfilePicture,
                    Country = u.Country,
                    Biography = u.Biography
                }).FirstOrDefaultAsync();

                var response = new GetResponse<UserAccountResponse>()
                {
                    Status = true,
                    Entity = user,
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<UserAccountResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<UserAccountResponse>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<AuthenticationResponse>>> GetUserAccountById(Guid userId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<AuthenticationResponse>>();
                var user = await _context.UserAccounts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

                var result = user.ToModel<AuthenticationResponse>();

                var response = new GetResponse<AuthenticationResponse>()
                {
                    Status = true,
                    Entity = result,
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AuthenticationResponse>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AuthenticationResponse>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<SearchReply<UserAccountModel>>> GetAllUserAccounts(SearchCall<SearchUserList> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 1;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            try
            {
                var apiResponse = new ApiResponse<SearchReply<UserAccountModel>>();
                var query = _context.UserAccounts.Select(u => new UserAccountModel
                {
                    UserId = u.UserId,
                    //FirstName = u.FirstName,
                    //LastName = u.LastName,
                    //Email = u.Email,
                    //ProfilePicture = u.ProfilePicture,
                    //Country = u.Country,
                    //Biography = u.Biography,
                    //UserRole = (RolesEnum)u.RoleId,
                    //Status = u.Disabled
                });

                //if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                //{
                //    query = query.Where(x => x.FirstName.Contains(options.Parameter.SearchQuery)
                //    || x.LastName.Contains(options.Parameter.SearchQuery)
                //     || x.Email.Contains(options.Parameter.SearchQuery));
                //}
                //if (options.Parameter.UserRole.HasValue)
                //{
                //    query = query.Where(x => x.UserRole.Equals(options.Parameter.UserRole));
                //}
                int offset = (pageNumber - 1) * pageSize;
                count = query.Count();
                var result = await query.OrderBy(x => x.Email).ThenBy(x => x.CustomUserCode).Take(pageSize).Skip(offset).ToListAsync();


                var response = new SearchReply<UserAccountModel>()
                {
                    TotalCount = count,
                    Result = result,
                    Errors = null
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<UserAccountModel>() { TotalCount = count, Result = new List<UserAccountModel>() }, IsSuccess = false };
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _context.UserAccounts.AnyAsync(x => x.Email == email))
                return true;

            return false;
        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }

        public bool ValidatePassword(string passWord)
        {
            int validConditions = 0;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            if (validConditions == 2)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=', '_', '-' }; // or whatever    
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            try
            {
                var (refreshToken, account) = getRefreshToken(token);

                // revoke token and save
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task UpdateLastActive(Guid userId)
        {
            try
            {
                var user = _context.UserAccounts.SingleOrDefault(x => x.UserId == userId);
                if(user != null)
                {
                    user.LastActive = DateTime.Now;
                    await _context.SaveChangesAsync();
                }     
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse<CreateResponse>> DisableEnableUser(string rawUserId, string rawLoggedInUserId)
        {
            try
            {
                var userId = Guid.Empty;
                var userIdNotEmpty = Guid.TryParse(rawUserId, out userId);
                if (userIdNotEmpty && userId != Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "User Id is required." }, IsSuccess = false };
                }
                var loggedInUserId = Guid.Empty;
                var loggedInUserIdNotEmpty = Guid.TryParse(rawLoggedInUserId, out userId);
                if (loggedInUserIdNotEmpty && loggedInUserId != Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Logged in user cannot disable/enable his/her self." }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await _context.UserAccounts.FirstOrDefaultAsync(x => x.UserId == userId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Disabled = !entity.Disabled;

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Enabled/Disabled User Id => {entity.UserId}: Name = {entity.FirstName} {entity.LastName}";
                            await _auditTrail.SaveAuditTrail(details, "User", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.UserId,
                    Message = "Record deleted successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        private (RefreshToken, UserAccount) getRefreshToken(string token)
        {
            var account = _context.UserAccounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new Exception("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new Exception("Invalid token");
            return (refreshToken, account);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private void removeOldRefreshTokens(UserAccount user)
        {
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(2) <= DateTime.UtcNow);
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task sendVerificationEmail(UserAccount account, string origin)
        {
            var admin = _context.UserAccounts.Where(x => x.Email.ToLower().Contains("workoffice")).FirstOrDefault();
            var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
            var messageParams = new Dictionary<string, string>(){
                {"firstName", account.FirstName},
                {"verifyURL", verifyUrl},
            };

            var mailTitle = $"WorkOffice Account Verification";
            List<SaveNotificationModel> saveNotifications = new List<SaveNotificationModel>();
            var notification = new SaveNotificationModel { SenderId = admin.UserId, ReceiverId = account.UserId, Title = mailTitle, Body = messageParams.ToString() };
            saveNotifications.Add(notification);
            await _auditTrail.SaveNotification(saveNotifications);
            await _emailService.VerificationEmail(account, verifyUrl);

        }


        private async Task SendPasswordResetEmail(UserAccount user, string origin)
        {
            var admin = _context.UserAccounts.Where(x => x.Email.ToLower().Contains("workoffice")).FirstOrDefault();
            var resetUrl = $"{origin}/account/reset-password?token={user.ResetToken}";
            var volunteerMessageParams = new Dictionary<string, string>(){
                {"firstName", user.FirstName},
                {"resetURL", resetUrl},
                {"userEmail", user.Email},
            };

            var mailTitle = $"Password Reset Request";
            List<SaveNotificationModel> saveNotifications = new List<SaveNotificationModel>();
            var notification = new SaveNotificationModel { SenderId = admin.UserId, ReceiverId = user.UserId, Title = mailTitle, Body = volunteerMessageParams.ToString() };
            saveNotifications.Add(notification);
            await _auditTrail.SaveNotification(saveNotifications);

            await _emailService.ResetPasswordEmail(user, resetUrl);

        }

        private async Task sendVerificationSuccessfulEmail(UserAccount account, string origin)
        {
            var admin = _context.UserAccounts.Where(x => x.Email.ToLower().Contains("workoffice")).FirstOrDefault();
            var loginUrl = $"{origin}/account/login";
            var messageParams = new Dictionary<string, string>(){
                {"firstName", account.FirstName},
                {"loginURL", loginUrl},
            };

            List<SaveNotificationModel> saveNotifications = new List<SaveNotificationModel>();
            var notification = new SaveNotificationModel { SenderId = admin.UserId, ReceiverId = account.UserId, Title = "Account Verified. Welcome to SkilledAlly", Body = messageParams.ToString() };
            saveNotifications.Add(notification);
            await _auditTrail.SaveNotification(saveNotifications);

            await _emailService.VolunterVerifiedEmail(account, loginUrl);

        }
    }
}