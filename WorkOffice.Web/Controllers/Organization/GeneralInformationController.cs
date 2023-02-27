using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H2RHRMS.Core.Interfaces.Services;
using H2RHRMS.Domain.Models;
using H2RHRMS.Domain.Models.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace H2RHRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralInformationController : ControllerBase
    {
        private readonly IGeneralInformationService service;

        public GeneralInformationController(IGeneralInformationService _service)
        {
            service = _service;
        }
        //  POST /api/GeneralInformation/Create
        /// <summary>
        /// Create GeneralInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/GeneralInformation/Create
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">GeneralInformation Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(GeneralInformationModel model)
        {
            try
            {
                model.ClientId = 1;
                var apiResponse = await service.Create(model);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(apiResponse.ResponseType);
                }
                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }

        // GET api/GeneralInformation/Get
        /// <summary>
        /// Get object of GeneralInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="generalInformationId"></param>
        /// <returns>Object of GeneralInformation</returns>
        /// <response code="200">Returns object of GeneralInformation</response>
        /// <response code="404">If object of GeneralInformation is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<GeneralInformationModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long generalInformationId)
        {
            try
            {
                var clientId = 1;
                var apiResponse = await service.Get(generalInformationId, clientId);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(apiResponse.ResponseType);
                }

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(apiResponse.ResponseType);
                }

                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }


        // GET api/GeneralInformation/Get
        /// <summary>
        /// Delete GeneralInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="generalInformationId"></param>
        /// <returns>Object of GeneralInformation</returns>
        /// <response code="200">Returns object of GeneralInformation</response>
        /// <response code="404">If object of GeneralInformation is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long generalInformationId)
        {
            try
            {
                var apiResponse = await service.Delete(generalInformationId);
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return BadRequest(apiResponse.ResponseType);
                }

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(apiResponse.ResponseType);
                }

                return Ok(apiResponse.ResponseType);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

       
    }
}