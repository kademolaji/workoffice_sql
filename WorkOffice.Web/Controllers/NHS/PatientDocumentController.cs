﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Common.Enums;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Web.Dtos;
using WorkOffice.Web.Filters;

namespace WorkOffice.Web.Controllers.NHS
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Authorize]
    public class PatientDocumentController : ControllerBase
    {
        private readonly IPatientDocumentService service;
        private readonly IUserAuthorizationService authorized;
        private readonly IHttpAccessorService httpAccessorService;
        public PatientDocumentController(IPatientDocumentService _service, IUserAuthorizationService _userAuthorization, IHttpAccessorService _httpAccessorService)
        {
            service = _service;
            authorized = _userAuthorization;
            httpAccessorService = _httpAccessorService;
        }
        //  POST /api/PatientDocument/Create
        /// <summary>
        /// Create PatientDocument
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/PatientDocument/Create
        ///     {
        ///       "revenueId": 10,
        ///       "customerId": 210
        ///         }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Created success message </returns>
        /// <response code="201">PatientDocument Created Successfully</response>
        /// <response code="400">If an error occur or invalid request payload</response>
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type = typeof(CreateResponse))]
        [ProducesResponseType(400, Type = typeof(CreateResponse))]
        public async Task<IActionResult> Create([FromForm] PatientDocumentDto model)
        {
            try
            {
                var CurrentUserName = httpAccessorService.GetCurrentUserName();
                var userId = httpAccessorService.GetCurrentUserId();
                var clientId = httpAccessorService.GetCurrentClientId();

                if (model.File == null || model.File.Length == 0)
                {
                    return Ok(new { Status = false, ProjectImage = "", Message = $"Please select image file" });
                }
                string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".svg", ".gif", ".pdf", ".txt", ".doc", ".docx" };
                string extension = Path.GetExtension(model.File.FileName);
                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    return Ok(new { Status = false, ProjectImage = "", Message = $"{model.File.FileName}'s file extension is not valid." });
                }
                if (model.File.Length > (5 * 1024 * 1024))
                {
                    return Ok(new { Status = false, ProjectImage = "", Message = $"{model.File.FileName}'s file size is bigger than 5MB." });
                }

                using var fileStream = model.File.OpenReadStream();
                byte[] bytes = new byte[model.File.Length];
                fileStream.Read(bytes, 0, (int)model.File.Length);

                if (authorized.CanPerformActionOnResource(userId, (long)UserActivitiesEnum.Patient_Document, clientId, UserActions.Add))
                {
                    var newModel = new PatientDocumentModel
                    {
                        DocumentTypeId = model.DocumentTypeId,
                        PatientId = model.PatientId,
                        PhysicalLocation = model.PhysicalLocation,
                        DocumentName = model.DocumentName,
                        DocumentExtension = model.File.ContentType,
                        DocumentFile = bytes,
                        ClinicDate = model.ClinicDate,
                        DateUploaded = DateTime.UtcNow,
                        SpecialityId = model.SpecialityId,
                    };

                        var apiResponse = await service.Create(newModel);
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
        // GET api/PatientDocument/GetList
        /// <summary>
        /// Get list of PatientDocument
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="options"></param>
        /// <returns>List of PatientDocument</returns>
        /// <response code="200">Returns list of PatientDocument</response>
        /// <response code="404">If list of PatientDocument is null</response> 
        /// <response code="400">If an error occur or invalid request payload</response> 
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetResponse<List<PatientDocumentModel>>))]
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



        // GET api/PatientDocument/Get
        /// <summary>
        /// Delete PatientDocument
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="patientId"></param>
        /// <returns>Object of PatientDocument</returns>
        /// <response code="200">Returns object of PatientDocument</response>
        /// <response code="404">If object of PatientDocument is null</response> 
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
        /// Delete Multiple PatientDocument
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Object of PatientDocument</returns>
        /// <response code="200">Returns object of PatientDocument</response>
        /// <response code="404">If object of PatientDocument is null</response> 
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

        [HttpGet]
        [Route("Download")]
        public async Task<FileStreamResult> DownloadPatientDocument(int patientDocumentId)
        {
            var myImage = await service.Get(patientDocumentId);
            var stream = new MemoryStream(myImage.ResponseType.Entity.DocumentFile);

            return new FileStreamResult(stream, myImage.ResponseType.Entity.DocumentExtension)
            {
                FileDownloadName = myImage.ResponseType.Entity.DocumentName
            };
        }
    }
}