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
    public class WaitingTypeController : ControllerBase
    {
        private readonly IWaitingTypeService _WaitingTypeService;
        private readonly IHttpAccessorService _httpAccessorService;
        public WaitingTypeController(IWaitingTypeService WaitingTypeService,
            IHttpAccessorService httpAccessorService
           )
        {
            _WaitingTypeService = WaitingTypeService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("waiting-type")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateWaitingType(WaitingTypeViewModels model)
        {
            try
            {
                var apiResponse = await _WaitingTypeService.CreateWaitingType(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
