using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Web.Filters;

namespace WorkOffice.Web.Controllers
{

    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Authorize]
    public class PatientInformationController : ControllerBase
    {
        private readonly IPatientInformationService service;
        private readonly IUserAuthorizationService authorized;
        private readonly IHttpAccessorService httpAccessorService;
        public PatientInformationController(IPatientInformationService _service, IUserAuthorizationService _userAuthorization, IHttpAccessorService _httpAccessorService)
        {
            service = _service;
            authorized = _userAuthorization;
            httpAccessorService = _httpAccessorService;
        }
        //  POST /api/PatientInformation/Create
        /// <summary>
        /// Create PatientInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/PatientInformation/Create
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">PatientInformation Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(PatientInformationModel model)
        {
            try
            {
                model.CurrentUserName = httpAccessorService.GetCurrentUserName();
                var userId = httpAccessorService.GetCurrentUserId();
                var clientId = httpAccessorService.GetCurrentClientId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Patient_Information, clientId, UserActions.Add))
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
        // GET api/PatientInformation/GetList
        /// <summary>
        /// Get list of PatientInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="options"></param>
        /// <returns>List of PatientInformation</returns>
        /// <response code="200">Returns list of PatientInformation</response>
        /// <response code="404">If list of PatientInformation is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<PatientInformationModel>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> GetList(SearchCall<SearchParameter> options)
        {
            try
            {
                var apiResponse = await service.GetList(options);
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

        // GET api/PatientInformation/Get
        /// <summary>
        /// Get object of PatientInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="patientId"></param>
        /// <returns>Object of PatientInformation</returns>
        /// <response code="200">Returns object of PatientInformation</response>
        /// <response code="404">If object of PatientInformation is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<PatientInformationModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long patientId)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var apiResponse = await service.Get(patientId);
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



        // GET api/PatientInformation/Get
        /// <summary>
        /// Delete PatientInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="patientId"></param>
        /// <returns>Object of PatientInformation</returns>
        /// <response code="200">Returns object of PatientInformation</response>
        /// <response code="404">If object of PatientInformation is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long patientId)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var userId = httpAccessorService.GetCurrentUserId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Patient_Information, clientId, UserActions.Delete))
                {
                    var apiResponse = await service.Delete(patientId);
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
        /// Delete Multiple PatientInformation
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Object of PatientInformation</returns>
        /// <response code="200">Returns object of PatientInformation</response>
        /// <response code="404">If object of PatientInformation is null</response> 
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
                var clientId = httpAccessorService.GetCurrentClientId();
                var userId = httpAccessorService.GetCurrentUserId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Patient_Information, clientId, UserActions.Delete))
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