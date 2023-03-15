﻿using Microsoft.AspNetCore.Authorization;
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
    public class PathwayStatusController : ControllerBase
    {
        private readonly IPathwayStatusService _PathwayStatusService;
        private readonly IHttpAccessorService _httpAccessorService;
        public PathwayStatusController(IPathwayStatusService PathwayStatusService,
            IHttpAccessorService httpAccessorService
           )
        {
            _PathwayStatusService = PathwayStatusService;
            _httpAccessorService = httpAccessorService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("pathway-status")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> CreatePathwayStatus(PathwayStatusViewModels model)
        {
            try
            {
                var apiResponse = await _PathwayStatusService.CreatePathwayStatus(model);
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
    }
}