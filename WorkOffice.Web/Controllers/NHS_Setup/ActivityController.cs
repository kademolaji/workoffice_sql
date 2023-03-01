using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly INHSActivityService _ActivityService;
        private readonly IHttpAccessorService _httpAccessorService;
        public ActivityController(INHSActivityService ActivityService,
            IHttpAccessorService httpAccessorService
           )
        {
            _ActivityService = ActivityService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("activity")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreateActivity(NHSActivityViewModels model)
        {
            try
            {
                var apiResponse = await _ActivityService.CreateActivity(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}
