using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class AppTypeController : ControllerBase
    {
        private readonly IAppTypeService _AppTypeService;
        private readonly IHttpAccessorService _httpAccessorService;
        public AppTypeController(IAppTypeService AppTypeService,
            IHttpAccessorService httpAccessorService
           )
        {
            _AppTypeService = AppTypeService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("app-type")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateAppType(AppTypeViewModels model)
        {
            try
            {
                var apiResponse = await _AppTypeService.CreateAppType(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
