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
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _ConsultantService;
        private readonly IHttpAccessorService _httpAccessorService;
        public ConsultantController(IConsultantService ConsultantService,
            IHttpAccessorService httpAccessorService
           )
        {
            _ConsultantService = ConsultantService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("consultant")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateConsultant(ConsultantViewModels model)
        {
            try
            {
                var apiResponse = await _ConsultantService.CreateConsultant(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
