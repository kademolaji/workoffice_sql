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
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _HospitalService;
        private readonly IHttpAccessorService _httpAccessorService;
        public HospitalController(IHospitalService HospitalService,
            IHttpAccessorService httpAccessorService
           )
        {
            _HospitalService = HospitalService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("hospital")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateHospital(HospitalViewModels model)
        {
            try
            {
                var apiResponse = await _HospitalService.CreateHospital(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
