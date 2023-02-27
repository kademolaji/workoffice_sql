using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using H2RHRMS.Api.Utilities;
using H2RHRMS.Core.Interfaces.Services;
using H2RHRMS.Domain.Models;
using H2RHRMS.Domain.Models.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace H2RHRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IlocationService service;

        public LocationController(IlocationService _service)
        {
            service = _service;
        }
        //  POST /api/Location/Create
        /// <summary>
        /// Create Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Location/Create
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">Location Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(LocationModel model)
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
        // GET api/Location/GetList
        /// <summary>
        /// Get list of Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of Location</returns>
        /// <response code="200">Returns list of Location</response>
        /// <response code="404">If list of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<LocationModel>>))]
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

        // GET api/Location/Get
        /// <summary>
        /// Get object of Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="locationId"></param>
        /// <returns>Object of Location</returns>
        /// <response code="200">Returns object of Location</response>
        /// <response code="404">If object of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<LocationModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long locationId)
        {
            try
            {
                var apiResponse = await service.Get(locationId);
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
        /// <summary>
        /// Export Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of Location</returns>
        /// <response code="200">Returns object of Location</response>
        /// <response code="404">If object of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Export")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<byte[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> GenerateExportStructureDefinition()
        {
            try
            {
                var apiResponse = await service.Export();
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
        /// <summary>
        /// Upload Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of Location</returns>
        /// <response code="200">Returns object of Location</response>
        /// <response code="404">If object of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CreateResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CreateResponse))]
        public async Task<IActionResult> UploadStructureDefinition([FromForm]UploadModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();

                if (model.file == null || model.file.Length <= 0)
                {
                    return BadRequest(new CreateResponse { Id = "", Message = "File upload is required", Status = false });
                }
                if (!Path.GetExtension(model.file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new CreateResponse { Id = "", Message = "Unsuported file format", Status = false });
                }
                using (var stream = new MemoryStream())
                {
                    await model.file.CopyToAsync(stream);
                    apiResponse = await service.Upload(stream.ToArray(), 1);
                }

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
        /// <summary>
        /// Delete Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="locationId"></param>
        /// <returns>Object of Location</returns>
        /// <response code="200">Returns object of Location</response>
        /// <response code="404">If object of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long locationId)
        {
            try
            {
                var apiResponse = await service.Delete(locationId);
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

        /// <summary>
        /// Delete Multiple Location
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Object of Location</returns>
        /// <response code="200">Returns object of Location</response>
        /// <response code="404">If object of Location is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("MultipleDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> MultipleDelete(MultipleDeleteModel model)
        {
            try
            {
                var apiResponse = await service.MultipleDelete(model);
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