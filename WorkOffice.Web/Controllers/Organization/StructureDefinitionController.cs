﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using H2RHRMS.Api.Filters;
using H2RHRMS.Api.Utilities;
using H2RHRMS.Core.Interfaces.Services;
using H2RHRMS.Core.Utilities;
using H2RHRMS.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace H2RHRMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    public class StructureDefinitionController : ControllerBase
    {
        private readonly IStructureDefinitionService service;
        private readonly IUserAuthorizationService authorized;

        public StructureDefinitionController(IStructureDefinitionService _service, IUserAuthorizationService _userAuthorization)
        {
            service = _service;
            authorized = _userAuthorization;
        }
        //  POST /api/StructureDefinition/Create
        /// <summary>
        /// Create StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/StructureDefinition/Create
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">StructureDefinition Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(StructureDefinitionModel model)
        {
            try
            {
                model.ClientId = 1;
                if (authorized.CanPerformActionOnResource(2, 2, model.ClientId, UserActions.Add))
                {
                    var apiResponse = await service.Create(model);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return BadRequest(apiResponse.ResponseType);
                    }
                    return Ok(apiResponse.ResponseType);
                }

                else
                {
                    return BadRequest(
                       new { Status = false, Message = $"You do not have enough right to add structure definition" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error {ex.Message}");
            }
        }
        // GET api/StructureDefinition/GetList
        /// <summary>
        /// Get list of StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of StructureDefinition</returns>
        /// <response code="200">Returns list of StructureDefinition</response>
        /// <response code="404">If list of StructureDefinition is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<StructureDefinitionModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> GetList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var clientId = 1;
                var apiResponse = await service.GetList(clientId, pageNumber, pageSize);
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

        // GET api/StructureDefinition/Get
        /// <summary>
        /// Get object of StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="structureDefinitionId"></param>
        /// <returns>Object of StructureDefinition</returns>
        /// <response code="200">Returns object of StructureDefinition</response>
        /// <response code="404">If object of StructureDefinition is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<StructureDefinitionModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long structureDefinitionId)
        {
            try
            {
                var clientId = 1;
                var apiResponse = await service.Get(structureDefinitionId, clientId);
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

        // GET api/StructureDefinition/Get
        /// <summary>
        /// Export StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of StructureDefinition</returns>
        /// <response code="200">Returns object of StructureDefinition</response>
        /// <response code="404">If object of StructureDefinition is null</response> 
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
                var clientId = 1;
                if (authorized.CanPerformActionOnResource(2, 2, clientId, UserActions.View))
                {
                    var apiResponse = await service.Export(clientId);
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

                else
                {
                    return BadRequest(
                       new { Status = false, Message = $"You do not have enough right to view structure definition" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        // GET api/StructureDefinition/Get
        /// <summary>
        /// Upload StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of StructureDefinition</returns>
        /// <response code="200">Returns object of StructureDefinition</response>
        /// <response code="404">If object of StructureDefinition is null</response> 
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
                var clientId = 1;
                if (authorized.CanPerformActionOnResource(2, 2, clientId, UserActions.Add))
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
                        apiResponse = await service.Upload(stream.ToArray(), clientId);
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

                else
                {
                    return BadRequest(
                       new { Status = false, Message = $"You do not have enough right to upload structure definition" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }


        // GET api/StructureDefinition/Get
        /// <summary>
        /// Delete StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="structureDefinitionId"></param>
        /// <returns>Object of StructureDefinition</returns>
        /// <response code="200">Returns object of StructureDefinition</response>
        /// <response code="404">If object of StructureDefinition is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long structureDefinitionId)
        {
            try
            {
                var clientId = 1;
                if (authorized.CanPerformActionOnResource(2, 2, clientId, UserActions.Delete))
                {
                    var apiResponse = await service.Delete(structureDefinitionId);
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

                else
                {
                    return BadRequest(
                       new { Status = false, Message = $"You do not have enough right to delete structure definition" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        /// <summary>
        /// Delete Multiple StructureDefinition
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Object of StructureDefinition</returns>
        /// <response code="200">Returns object of StructureDefinition</response>
        /// <response code="404">If object of StructureDefinition is null</response> 
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
                var clientId = 1;
                if (authorized.CanPerformActionOnResource(2, 2, clientId, UserActions.Delete))
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
                else
                {
                    return BadRequest(
                       new { Status = false, Message = $"You do not have enough right to delete structure definition" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
    }
}