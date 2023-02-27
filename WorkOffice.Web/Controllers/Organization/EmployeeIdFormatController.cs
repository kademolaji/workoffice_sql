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
    public class EmployeeIdFormatController : ControllerBase
    {
        private readonly IEmployeeIdFormatService service;

        public EmployeeIdFormatController(IEmployeeIdFormatService _service)
        {
            service = _service;
        }
        //  POST /api/EmployeeIdFormat/Create
        /// <summary>
        /// Create EmployeeIdFormat
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/EmployeeIdFormat/Create
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">EmployeeIdFormat Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(EmployeeIdFormatModel model)
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
        // GET api/EmployeeIdFormat/GetList
        /// <summary>
        /// Get list of EmployeeIdFormat
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of EmployeeIdFormat</returns>
        /// <response code="200">Returns list of EmployeeIdFormat</response>
        /// <response code="404">If list of EmployeeIdFormat is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<EmployeeIdFormatModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> GetList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var apiResponse = await service.GetList(pageNumber, pageSize);
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

        // GET api/EmployeeIdFormat/Get
        /// <summary>
        /// Get object of EmployeeIdFormat
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="employeeIdFormatId"></param>
        /// <returns>Object of EmployeeIdFormat</returns>
        /// <response code="200">Returns object of EmployeeIdFormat</response>
        /// <response code="404">If object of EmployeeIdFormat is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<EmployeeIdFormatModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long employeeIdFormatId)
        {
            try
            {
                var apiResponse = await service.Get(employeeIdFormatId);
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

        // GET api/Location/Get

        // GET api/EmployeeIdFormat/Get
        /// <summary>
        /// Delete EmployeeIdFormat
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="employeeIdFormatId"></param>
        /// <returns>Object of EmployeeIdFormat</returns>
        /// <response code="200">Returns object of EmployeeIdFormat</response>
        /// <response code="404">If object of EmployeeIdFormat is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long employeeIdFormatId)
        {
            try
            {
                var apiResponse = await service.Delete(employeeIdFormatId);
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