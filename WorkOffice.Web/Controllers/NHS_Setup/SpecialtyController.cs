using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _SpecialtyService;
        private readonly IHttpAccessorService _httpAccessorService;
        public SpecialtyController(ISpecialtyService SpecialtyService,
            IHttpAccessorService httpAccessorService
           )
        {
            _SpecialtyService = SpecialtyService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("specialty")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateSpecialty(SpecialtyViewModels model)
        {
            try
            {
                var apiResponse = await _SpecialtyService.CreateSpecialty(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
