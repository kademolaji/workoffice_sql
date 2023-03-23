using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Web.Utilities;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyStructureController : ControllerBase
    {
        private readonly ICompanyStructureService service;
        private readonly IHttpAccessorService httpAccessorService;

        public CompanyStructureController(ICompanyStructureService _service, IHttpAccessorService _httpAccessorService)
        {
            service = _service;
            httpAccessorService = _httpAccessorService;
        }
        //  POST /api/CompanyStructure/Create
        /// <summary>
        /// Create CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/CompanyStructure/Create
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">CompanyStructure Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(CompanyStructureModel model)
        {
            try
            {
                model.ClientId = httpAccessorService.GetCurrentClientId();
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
        // GET api/CompanyStructure/GetList
        /// <summary>
        /// Get list of CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="options"></param>
        /// <returns>List of CompanyStructure</returns>
        /// <response code="200">Returns list of CompanyStructure</response>
        /// <response code="404">If list of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchReply<CompanyStructureModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> GetList(SearchCall<SearchParameter> options)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var apiResponse = await service.GetList(options, clientId);
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

        // GET api/CompanyStructure/Get
        /// <summary>
        /// Get object of CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="companyStructureId"></param>
        /// <returns>Object of CompanyStructure</returns>
        /// <response code="200">Returns object of CompanyStructure</response>
        /// <response code="404">If object of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<CompanyStructureModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long companyStructureId)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var apiResponse = await service.Get(companyStructureId, clientId);
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

        // GET api/CompanyStructure/Get
        /// <summary>
        /// Export CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of CompanyStructure</returns>
        /// <response code="200">Returns object of CompanyStructure</response>
        /// <response code="404">If object of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Export")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<byte[]>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Export()
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
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
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        // GET api/StructureDefinition/Get
        /// <summary>
        /// Upload CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <returns>Object of CompanyStructure</returns>
        /// <response code="200">Returns object of CompanyStructure</response>
        /// <response code="404">If object of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CreateResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Upload([FromForm]UploadModel model)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
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
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }


        // GET api/CompanyStructure/Get
        /// <summary>
        /// Delete CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="companyStructureId"></param>
        /// <returns>Object of CompanyStructure</returns>
        /// <response code="200">Returns object of CompanyStructure</response>
        /// <response code="404">If object of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long companyStructureId)
        {
            try
            {
                var apiResponse = await service.Delete(companyStructureId);
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
        /// Delete Multiple CompanyStructure
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Object of CompanyStructure</returns>
        /// <response code="200">Returns object of CompanyStructure</response>
        /// <response code="404">If object of CompanyStructure is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
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