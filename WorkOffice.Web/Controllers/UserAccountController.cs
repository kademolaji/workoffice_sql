
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkOffice.Common;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts.Shared;
using WorkOffice.Contracts.ServicesContracts.Users;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;
using WorkOffice.Web.Dtos;
using WorkOffice.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHttpAccessorService _httpAccessorService;

        public UserAccountController(
            IUserAccountService userAccountService,
           IJwtTokenGenerator jwtTokenGenerator,
            IHttpAccessorService httpAccessorService
           )
        {
            _userAccountService = userAccountService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var apiResponse = await _userAccountService.Login(model.UserName, model.Password, ipAddress());
                if (apiResponse.ResponseType.Entity != null)
                {
                    setTokenCookie(apiResponse.ResponseType.Entity.RefreshToken);
                    var token = _jwtTokenGenerator.CreateToken(apiResponse.ResponseType.Entity);
                    apiResponse.ResponseType.Entity.Token = token;
                }
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateUpdate(RegisterRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.Register(model, model.Password, Request.Headers["origin"], ipAddress());
                if (apiResponse.ResponseType.Entity != null)
                {
                    setTokenCookie(apiResponse.ResponseType.Entity.RefreshToken);
                    var token = _jwtTokenGenerator.CreateToken(apiResponse.ResponseType.Entity);
                    apiResponse.ResponseType.Entity.Token = token;
                }
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("create-admin-user")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateAdminUser(CreateAdminUserModel model)
        {
            try
            {
                var apiResponse = await _userAccountService.CreateAdminUser(model, Request.Headers["origin"]);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update-user")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.UpdateUser(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("change-password")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                var apiResponse = await _userAccountService.ChangePassword(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var apiResponse = await _userAccountService.RefreshToken(refreshToken, ipAddress());
              
                if (apiResponse.ResponseType.Entity != null)
                {
                    setTokenCookie(apiResponse.ResponseType.Entity.RefreshToken);
                    var token = _jwtTokenGenerator.CreateToken(apiResponse.ResponseType.Entity);
                    apiResponse.ResponseType.Entity.Token = token;
                }
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                    return Ok(new { status = false, message = "Token is required" });

                await _userAccountService.RevokeToken(token, ipAddress());
                return Ok(new { status = true, message = "Token revoked" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.VerifyEmail(model.Token, Request.Headers["origin"]);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(apiResponse.ResponseType);
                }
                return Ok(new { message = "Verification successful, you can now login" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.ForgotPassword(model, Request.Headers["origin"]);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.ResetPassword(model);
              
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken(ValidateResetTokenRequest model)
        {
            try
            {
                var apiResponse = await _userAccountService.ValidateResetToken(model);
               
                return Ok(new { status = apiResponse.ResponseType.Status,  message = apiResponse.ResponseType.Message });
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{userId}/user-details")]
        public async Task<IActionResult> GetUserById(string requestUserId)
        {
            try
            {
                var userId = Guid.Empty;
                var userIdExist = Guid.TryParse(requestUserId, out userId);
                var apiResponse = await _userAccountService.GetUserById(userId);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{userId}/user-account")]
        public async Task<IActionResult> GetUserAccountById(string requestUserId)
        {
            try
            {
                var userId = Guid.Empty;
                var userIdExist = Guid.TryParse(requestUserId, out userId);
                var apiResponse = await _userAccountService.GetUserAccountById(userId);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("all-users")]
        public async Task<IActionResult> GetAllUsers(SearchCall<SearchUserList> options)
        {
            try
            {
                var apiResponse = await _userAccountService.GetAllUsers(options);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{id:int}/disable-enable")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> DisableEnableUser(string id)
        {
            try
            {
                var loggedInUser = _httpAccessorService.GetCurrentUserId();
                var apiResponse = await _userAccountService.DisableEnableUser(id, loggedInUser);

                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}