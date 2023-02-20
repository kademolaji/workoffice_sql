using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.Admin;
using WorkOffice.Contracts.ServicesContracts.Admin;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdministrationService _administrationService;
        private readonly IHttpAccessorService _httpAccessorService;
        public AdminController(IAdministrationService administrationService,
            IHttpAccessorService httpAccessorService
           )
        {
            _administrationService = administrationService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("user-role")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateUserRole(CreateUserRoleRequest model)
        {
            try
            {
                var apiResponse = await _administrationService.CreateUserRole(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
