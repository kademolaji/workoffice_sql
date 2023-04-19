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
        public class WaitingListController : ControllerBase
        {
            private readonly IWaitingListService service;
            private readonly IUserAuthorizationService authorized;
            private readonly IHttpAccessorService httpAccessorService;
            public WaitingListController(IWaitingListService _service, IUserAuthorizationService _userAuthorization, IHttpAccessorService _httpAccessorService)
            {
                service = _service;
                authorized = _userAuthorization;
                httpAccessorService = _httpAccessorService;
            }
            //  POST /api/WaitingList/Create
            /// <summary>
            /// Create WaitingList
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            ///     POST /api/WaitingList/Create
            ///     {
            ///       "revenueId": 10,
            ///       "customerId": 210
            ///         }
            ///
            /// </remarks>
            /// <param name="model"></param>
            /// <returns>Created success message </returns>
            /// <response code="201">WaitingList Created Successfully</response>
            /// <response code="400">If an error occur or invalid request payload</response>
            [HttpPost]
            [Route("Create")]
            [ProducesResponseType(201, Type = typeof(CreateResponse))]
            [ProducesResponseType(400, Type = typeof(CreateResponse))]
            public async Task<IActionResult> Create(WaitingListModel model)
            {
                try
                {
                    model.CurrentUsername = httpAccessorService.GetCurrentUserName();
                    var userId = httpAccessorService.GetCurrentUserId();
                    var clientId = httpAccessorService.GetCurrentClientId();
                    if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Add_Waitinglist, clientId, UserActions.Add))
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
            // GET api/WaitingList/GetList
            /// <summary>
            /// Get list of WaitingList
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            /// </remarks>
            /// <param name="options"></param>
            /// <returns>List of WaitingList</returns>
            /// <response code="200">Returns list of WaitingList</response>
            /// <response code="404">If list of WaitingList is null</response> 
            /// <response code="400">If an error occur or invalid request payload</response> 
            [HttpPost]
            [Route("GetList")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<WaitingListModel>>))]
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

        // GET api/WaitingList/Get
        /// <summary>
        /// Get object of WaitingList
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="waitinglistId"></param>
        /// <returns>Object of WaitingList</returns>
        /// <response code="200">Returns object of WaitingList</response>
        /// <response code="404">If object of WaitingList is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpGet]
            [Route("Get")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<WaitingListModel>))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GetResponse<ProducesResponseStub>))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GetResponse<ProducesResponseStub>))]
            public async Task<IActionResult> Get(long waitinglistId)
            {
                try
                {
                    var clientId = httpAccessorService.GetCurrentClientId();
                    var apiResponse = await service.Get(waitinglistId);
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



        // GET api/WaitingList/Get
        /// <summary>
        /// Delete WaitingList
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="waitinglistId"></param>
        /// <returns>Object of WaitingList</returns>
        /// <response code="200">Returns object of WaitingList</response>
        /// <response code="404">If object of WaitingList is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpDelete]
            [Route("Delete")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteReply))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DeleteReply))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DeleteReply))]
            public async Task<IActionResult> Delete(long waitinglistId)
            {
                try
                {
                    var clientId = httpAccessorService.GetCurrentClientId();
                    var userId = httpAccessorService.GetCurrentUserId();
                    if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Add_Waitinglist, clientId, UserActions.Delete))
                    {
                        var apiResponse = await service.Delete(waitinglistId);
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
            /// Delete Multiple WaitingList
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///
            /// </remarks>
            /// <param name="model"></param>
            /// <returns>Object of WaitingList</returns>
            /// <response code="200">Returns object of WaitingList</response>
            /// <response code="404">If object of WaitingList is null</response> 
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
                    if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Add_Waitinglist, clientId, UserActions.Delete))
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