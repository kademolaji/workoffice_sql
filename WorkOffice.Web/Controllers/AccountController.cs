//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using WorkOffice.Contracts.Models;
//using WorkOffice.Contracts.ServicesContracts;
//using WorkOffice.Web.Utilities;

//namespace WorkOffice.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly IAuthenticationService service;
//        private readonly GenerateTokenExtentions generateToken;

//        public AccountController(IAuthenticationService _service, GenerateTokenExtentions _generateToken )
//        {
//            service = _service;
//            this.generateToken = _generateToken;
//        }
//        //  POST /api/Account/Login
//        /// <summary>
//        /// Login
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST /api/CompanyStructure/Create
//        ///
//        /// </remarks>
//        /// <param name="model"></param>
//        /// <returns>Created success message </returns>
//        /// <response code="201">CompanyStructure Created Successfully</response>
//        /// <response code="400">If an error occur or invalid request payload</response>
//        [HttpPost]
//        [Route("Login")]
//        [ProducesResponseType(201, Type = typeof(CreateResponse))]
//        [ProducesResponseType(400, Type = typeof(CreateResponse))]
//        public async Task<IActionResult> Login(LoginModel model)
//        {
//            try
//            {
//                var apiResponse = await service.Login(model.UserName, model.Password);
//                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
//                {
//                    return BadRequest(apiResponse.ResponseType);
//                }
//                if(apiResponse.ResponseType.Entity != null)
//                {
//                    var token = generateToken.GenerateToken(apiResponse.ResponseType.Entity);
//                    var roles = service.GetUserActivitiesByUser(apiResponse.ResponseType.Entity.UserId);
//                    return Ok(new { access_token = token, roles = roles,  status = true, message = "User logged in successfully" });
//                }
//                return Unauthorized(new { access_token = "", roles = "", status = false, message = "User log in failed" });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Server Error {ex.Message}");
//            }
//        }
//        //  POST /api/Account/Login
//        /// <summary>
//        /// Login
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST /api/CompanyStructure/Create
//        ///
//        /// </remarks>
//        /// <param name="model"></param>
//        /// <returns>Created success message </returns>
//        /// <response code="201">CompanyStructure Created Successfully</response>
//        /// <response code="400">If an error occur or invalid request payload</response>
//        [HttpPost]
//        [Route("Register")]
//        [ProducesResponseType(201, Type = typeof(CreateResponse))]
//        [ProducesResponseType(400, Type = typeof(CreateResponse))]
//        public async Task<IActionResult> Create(UserAccountModel model)
//        {
//            try
//            {
//                model.ClientId = 1;
//                var apiResponse = await service.Create(model, "password");
//                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
//                {
//                    return BadRequest(apiResponse.ResponseType);
//                }
//                return Ok(apiResponse.ResponseType);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Server Error {ex.Message}");
//            }
//        }
//    }
//}