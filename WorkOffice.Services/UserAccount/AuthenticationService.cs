
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Mappings;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public AuthenticationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }
        public async Task<ApiResponse<GetResponse<UserAccountModel>>> Login(string userName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    return new ApiResponse<GetResponse<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<UserAccountModel> { Status = false, Entity = null, Message = "UsewrName is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(password))
                {
                    return new ApiResponse<GetResponse<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<UserAccountModel> { Status = false, Entity = null, Message = "Password is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<UserAccountModel>>();

                var result = await context.UserAccounts.FirstOrDefaultAsync(x => x.Email == userName);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<UserAccountModel>() { Status = false, Message = "Invalid login credentials" }, IsSuccess = false };
                }

                if (!VerifyPasswordHash(password, result.PasswordHash, result.PasswordSalt))
                {
                    return new ApiResponse<GetResponse<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<UserAccountModel>() { Status = false, Message = "Invalid login credentials" }, IsSuccess = false };
                }


                var response = new GetResponse<UserAccountModel>()
                {
                    Status = true,
                    Entity = result.ToModel<UserAccountModel>(),
                    Message = "User logged in successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<UserAccountModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<UserAccountModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
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


        public async Task<ApiResponse<CreateResponse>> Create(UserAccountModel model, string password)
        {
            try
            {
                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Name is required." }, IsSuccess = false };
                }
                if (await UserExists(model.UserName, model.UserId))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.UserName} already exist." }, IsSuccess = false };
                }
                var emailAddress = new EmailAddressAttribute();
                if (emailAddress.IsValid(model.Email))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Invalid email {model.UserName}." }, IsSuccess = false };
                }
                if (await EmailExists(model.UserName, model.UserId))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.UserName} already exist." }, IsSuccess = false };
                }
               

                List<UserAccess> userAccess = new List<UserAccess>();
                List<UserAccountRole> userAccountRole = new List<UserAccountRole>();
                List<UserAccountAdditionalActivity> userActivities = new List<UserAccountAdditionalActivity>();

                if (model.Activities.Any())
                {
                    foreach (var item in model.Activities)
                    {
                        var userActivity = new UserAccountAdditionalActivity()
                        {
                            UserActivityId = item.UserActivityId,
                            CanAdd = item.CanAdd,
                            CanEdit = item.CanEdit,
                            CanApprove = item.CanApprove,
                            CanView = item.CanView,
                            CanDelete = item.CanDelete,
                            ClientId = model.ClientId
                        };

                        userActivities.Add(userActivity);
                    }
                }

                if (model.UserAccessIds.Any())
                {
                    foreach (var item in model.UserAccessIds)
                    {
                        var access = new UserAccess()
                        {
                            CompanyStructureId = item,
                            ClientId = model.ClientId
                        };
                        userAccess.Add(access);
                    }
                }
                if (model.UserRoleIds.Any())
                {
                    foreach (var item in model.UserRoleIds)
                    {
                        var access = new UserAccountRole()
                        {
                            UserRoleDefinitionId = item,
                            ClientId = model.ClientId
                        };
                        userAccountRole.Add(access);
                    }
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                UserAccount entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.UserId != Guid.Empty)
                        {
                            entity = context.UserAccounts.Find(model.UserId);
                            if (entity != null)
                            {
                                var targetUserAccesses = context.UserAccesses.Where(x => x.UserAccountId == entity.UserId).ToList();
                                var targetUserRoles = context.UserAccountRoles.Where(x => x.UserAccountId == entity.UserId).ToList();
                                var targetActivities = context.UserAccountAdditionalActivities.Where(x => x.UserAccountId == entity.UserId).ToList();

                                if (targetUserAccesses.Any())
                                {
                                    context.UserAccesses.RemoveRange(targetUserAccesses);
                                }
                                if (targetUserRoles.Any())
                                {
                                    context.UserAccountRoles.RemoveRange(targetUserRoles);
                                }
                                if (targetActivities.Any())
                                {
                                    context.UserAccountAdditionalActivities.RemoveRange(targetActivities);
                                }
                                //entity.Name = model.Name;
                                //entity.UserName = model.UserName;
                                //entity.EmployeeId = model.EmployeeId;
                                //entity.Isemployee = model.Isemployee;
                                //entity.Status = model.Status;
                                //entity.Role = model.Role;
                                entity.Email = model.Email;
                                entity.PhoneNumber = model.PhoneNumber;
                                entity.PhoneNumberConfirmed = true;
                                entity.TwoFactorEnabled = false;
                                entity.LockoutEnabled = true;
                                entity.AccessFailedCount = 0;
                                entity.IsFirstLoginAttempt = true;
                                entity.SecurityQuestion = model.SecurityQuestion;
                                entity.SecurityAnswer = model.SecurityAnswer;
                                entity.NextPasswordChangeDate = DateTime.Now.AddDays(30);
                                entity.UserAccess = userAccess;
                                entity.UserAccountRole = userAccountRole;
                                entity.UserAccountAdditionalActivity = userActivities;
                            }
                        }
                        else
                        {
                            byte[] passwordHash, passwordSalt;
                            CreatePasswordHash(password, out passwordHash, out passwordSalt);
                            entity = new UserAccount
                            {
                                //Name = model.Name,
                                //UserName = model.UserName,
                              
                                //Status = model.Status,
                                //Role = model.Role,
                                Email = model.Email,
                                EmailConfirmed = true,
                                PasswordHash = passwordHash,
                                PasswordSalt = passwordSalt,
                                PhoneNumber = model.PhoneNumber,
                                PhoneNumberConfirmed = true,
                                TwoFactorEnabled = false,
                                LockoutEnd = DateTime.Now.AddHours(48),
                                LockoutEnabled = true,
                                AccessFailedCount = 0,
                                IsFirstLoginAttempt = true,
                                SecurityQuestion = model.SecurityQuestion,
                                SecurityAnswer = model.SecurityAnswer,
                                NextPasswordChangeDate = DateTime.Now.AddDays(30),
                                ClientId = model.ClientId,
                                CanChangePassword = true,
                                Accesslevel = model.Accesslevel,
                                UserAccess = userAccess,
                                UserAccountRole = userAccountRole,
                                UserAccountAdditionalActivity = userActivities,
                            };
                            context.UserAccounts.Add(entity);
                        }

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new Uswer: Definition = {model.ClientId}, Description = {model.UserName}, Level = {model.Name} ";
                            await auditTrail.SaveAuditTrail(details, "User", "Create");
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username, Guid userId)
        {
            if (await context.UserAccounts.AnyAsync(o => o.Email.ToLower().Equals(username.ToLower()) && o.UserId != userId))
                return true;

            return false;
        }
        public async Task<bool> EmailExists(string email, Guid userId)
        {
            if (await context.UserAccounts.AnyAsync(x => x.Email == email && x.UserId != userId))
                return true;

            return false;
        }

        public List<string> GetUserActivitiesByUser(Guid userAccountId)
        {
            List<string> activities = new List<string>();

            var user = context.UserAccounts.Where(x => x.UserId == userAccountId).FirstOrDefault();

            var additionalActivityIds = context.UserAccountAdditionalActivities.Where(x => x.UserAccountId == user.UserId).Select(x => x.UserActivityId);

            var userRoleActivityIds = (from a in context.UserAccountRoles
                                       join b in context.UserRoleDefinitions on a.UserRoleDefinitionId equals b.UserRoleDefinitionId
                                       join c in context.UserRoleActivities on b.UserRoleDefinitionId equals c.UserRoleDefinitionId
                                       where a.UserAccountId == user.UserId
                                       select c.UserActivityId).ToList();


            activities = context.UserActivities.Where(x => additionalActivityIds.Contains(x.UserActivityId)
               || userRoleActivityIds.Contains(x.UserActivityId)
              ).Select(x => x.UserActivityName.ToLower()).ToList();

            return activities;
        }
    }
}
