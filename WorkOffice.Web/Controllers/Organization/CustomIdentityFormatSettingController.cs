using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomIdentityFormatSettingController : ControllerBase
    {
        private readonly ICustomIdentityFormatSettingService service;
        private readonly IHttpAccessorService httpAccessorService;
        public CustomIdentityFormatSettingController(ICustomIdentityFormatSettingService _service, IHttpAccessorService _httpAccessorService)
        {
            service = _service;
            httpAccessorService = _httpAccessorService;
        }
        //  POST /api/customIdentityFormatSetting/Create
        /// <summary>
        /// Create customIdentityFormatSetting
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/customIdentityFormatSetting/Create
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">customIdentityFormatSetting Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(CustomIdentityFormatSettingModel model)
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
        // GET api/customIdentityFormatSetting/GetList
        /// <summary>
        /// Get list of customIdentityFormatSetting
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns>List of customIdentityFormatSetting</returns>
        /// <response code="200">Returns list of customIdentityFormatSetting</response>
        /// <response code="404">If list of customIdentityFormatSetting is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<CustomIdentityFormatSettingModel>>))]
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

        // GET api/customIdentityFormatSetting/Get
        /// <summary>
        /// Get object of customIdentityFormatSetting
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="customIdentityFormatSettingId"></param>
        /// <returns>Object of customIdentityFormatSetting</returns>
        /// <response code="200">Returns object of customIdentityFormatSetting</response>
        /// <response code="404">If object of customIdentityFormatSetting is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<CustomIdentityFormatSettingModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long customIdentityFormatSettingId)
        {
            try
            {
                var apiResponse = await service.Get(customIdentityFormatSettingId);
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


        // GET api/customIdentityFormatSetting/Get
        /// <summary>
        /// Delete customIdentityFormatSetting
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="customIdentityFormatSettingId"></param>
        /// <returns>Object of customIdentityFormatSetting</returns>
        /// <response code="200">Returns object of customIdentityFormatSetting</response>
        /// <response code="404">If object of customIdentityFormatSetting is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long customIdentityFormatSettingId)
        {
            try
            {
                var apiResponse = await service.Delete(customIdentityFormatSettingId);
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