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

namespace WorkOffice.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientValidationDetailsController : ControllerBase
    {
        private readonly IPatientValidationDetailsService service;
        private readonly IUserAuthorizationService authorized;
        private readonly IHttpAccessorService httpAccessorService;
        public PatientValidationDetailsController(IPatientValidationDetailsService _service, IUserAuthorizationService _userAuthorization, IHttpAccessorService _httpAccessorService)
        {
            service = _service;
            authorized = _userAuthorization;
            httpAccessorService = _httpAccessorService;
        }
        //  POST /api/PatientValidationDetails/Create
        /// <summary>
        /// Create PatientValidationDetails
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/PatientValidationDetails/Create
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">PatientValidationDetails Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create(PatientValidationDetailsModel model)
        {
            try
            {
                model.CurrentUserName = httpAccessorService.GetCurrentUserName();
                var userId = httpAccessorService.GetCurrentUserId();
                var clientId = httpAccessorService.GetCurrentClientId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Add_Pathway, clientId, UserActions.Add))
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

        //  POST /api/PatientValidationDetails/Merge
        /// <summary>
        /// Create PatientValidationDetails
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/PatientValidationDetails/Merge
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">PatientValidationDetails Merge Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Merge")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Merge(MergePathwayModel model)
        {
            try
            {
                model.CurrentUserName = httpAccessorService.GetCurrentUserName();
                var userId = httpAccessorService.GetCurrentUserId();
                var clientId = httpAccessorService.GetCurrentClientId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Add_Pathway, clientId, UserActions.Add))
                {
                    var apiResponse = await service.Merge(model);
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
        // GET api/PatientValidationDetails/GetList
        /// <summary>
        /// Get list of PatientValidationDetails
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="options"></param>
        /// <returns>List of PatientValidationDetails</returns>
        /// <response code="200">Returns list of PatientValidationDetails</response>
        /// <response code="404">If list of PatientValidationDetails is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<PatientValidationModel>>))]
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

        // GET api/PatientValidationDetails/Get
        /// <summary>
        /// Get object of PatientValidationDetails
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="patientValidationDetailsId"></param>
        /// <returns>Object of PatientValidationDetails</returns>
        /// <response code="200">Returns object of PatientValidationDetails</response>
        /// <response code="404">If object of PatientValidationDetails is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<PatientValidationModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
        public async Task<IActionResult> Get(long patientValidationDetailsId)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var apiResponse = await service.Get(patientValidationDetailsId);
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

        // GET api/PatientValidationDetails/Get
        /// <summary>
        /// Delete PatientValidationDetails
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="patientValidationDetailsId"></param>
        /// <returns>Object of PatientValidationDetails</returns>
        /// <response code="200">Returns object of PatientValidationDetails</response>
        /// <response code="404">If object of PatientValidationDetails is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
        public async Task<IActionResult> Delete(long patientValidationDetailsId)
        {
            try
            {
                var clientId = httpAccessorService.GetCurrentClientId();
                var userId = httpAccessorService.GetCurrentUserId();
                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Validate_now, clientId, UserActions.Delete))
                {
                    var apiResponse = await service.DeletePatientDetailsValidation(patientValidationDetailsId);
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
