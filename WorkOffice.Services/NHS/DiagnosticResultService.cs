using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class DiagnosticResultService : IDiagnosticResultService
    {

        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public DiagnosticResultService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(DiagnosticResultModel model)
        {
            try
            {

                if (model.DiagnosticResultId > 0)
                {
                    return await Update(model);
                }
                if (string.IsNullOrEmpty(model.DocumentName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Document Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.DocumentExtension))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Document Extension is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_DiagnosticResult entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_DiagnosticResult
                        {
                            DiagnosticId = model.DiagnosticId,
                            PatientId = model.PatientId,
                            ConsultantName = model.ConsultantName,
                            DocumentName = model.DocumentName,
                            DocumentExtension = model.DocumentExtension,
                            DocumentFile = model.DocumentFile,
                            TestResultDate = model.TestResultDate,
                            DateUploaded = model.DateUploaded,
                            SpecialityId = model.SpecialityId,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_DiagnosticResults.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Diagnostic Resultt: DocumentName = {model.DocumentName}, DocumentExtension = {model.DocumentExtension}, DiagnosticResultId = {model.DiagnosticResultId} ";
                            await auditTrail.SaveAuditTrail(details, "Diagnostic Result", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.DiagnosticResultId,
                    Message = "Record created successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> Update(DiagnosticResultModel model)
        {
            try
            {


                if (string.IsNullOrEmpty(model.DocumentName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Document Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.DocumentExtension))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Document Extension is required." }, IsSuccess = false };
                }

                var existingNHSNumber = context.NHS_DiagnosticResults.Any(x => x.Problem == model.Problem && x.DiagnosticResultId != model.DiagnosticResultId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another document already exists with the given document name" }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_DiagnosticResults.FindAsync(model.DiagnosticResultId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.DiagnosticId = model.DiagnosticId;
                        entity.PatientId = model.PatientId;
                        entity.ConsultantName = model.ConsultantName;
                        entity.DocumentName = model.DocumentName;
                        entity.DocumentExtension = model.DocumentExtension;
                        entity.DocumentFile = model.DocumentFile;
                        entity.TestResultDate = model.TestResultDate;
                        entity.DateUploaded = model.DateUploaded;
                        entity.SpecialityId = model.SpecialityId;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Diagnostic Result: DocumentName = {model.DocumentName}, DocumentExtension = {model.DocumentExtension}, DateUploaded = {model.DateUploaded} ";
                            await auditTrail.SaveAuditTrail(details, "Diagnostic Result", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.PatientId,
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<DiagnosticResultModel>>> Get(long DiagnosticResultId)
        {

            try
            {
                var apiResponse = new ApiResponse<GetResponse<DiagnosticResultModel>>();


                var result = await (from doc in context.NHS_DiagnosticResults
                                    where doc.DiagnosticResultId == DiagnosticResultId
                                    select new DiagnosticResultModel
                                    {
                                        DiagnosticResultId = doc.DiagnosticResultId,
                                        DiagnosticId = doc.DiagnosticId,
                                        PatientId = doc.PatientId,
                                        ConsultantName = doc.ConsultantName,
                                        DocumentName = doc.DocumentName,
                                        DocumentExtension = doc.DocumentExtension,
                                        DocumentFile = doc.DocumentFile,
                                        TestResultDate = doc.TestResultDate,
                                        DateUploaded = doc.DateUploaded,
                                        SpecialityId = doc.SpecialityId,

                                    }).FirstOrDefaultAsync();


                var response = new GetResponse<DiagnosticResultModel>()
                {
                    Status = true,
                    Entity = result
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<DiagnosticResultModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<DiagnosticResultModel>() { Status = false, Message = ex.Message }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<SearchReply<DiagnosticResultModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "documentname" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<DiagnosticResultModel>>();


                IQueryable<DiagnosticResultModel> query = (from doc in context.NHS_DiagnosticResults
                                                          where doc.PatientId == int.Parse(options.Parameter.SearchQuery)
                                                          select new DiagnosticResultModel
                                                          {
                                                              DiagnosticResultId = doc.DiagnosticResultId,
                                                              DiagnosticId = doc.DiagnosticId,
                                                              PatientId = doc.PatientId,
                                                              ConsultantName = doc.ConsultantName,
                                                              DocumentName = doc.DocumentName,
                                                              DocumentExtension = doc.DocumentExtension,
                                                              DocumentFile = doc.DocumentFile,
                                                              TestResultDate = doc.TestResultDate,
                                                              DateUploaded = doc.DateUploaded,
                                                              SpecialityId = doc.SpecialityId,
                                                              Speciality = doc.SpecialityId != null ? context.Specialties.FirstOrDefault(x => x.SpecialtyId == doc.SpecialityId).Name : "",
   

                                                          }).AsQueryable();
                int offset = (pageNumber) * pageSize;


                switch (sortField)
                {
                    case "documentname":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.DocumentName) : query.OrderByDescending(s => s.DocumentName);
                        break;
                    case "dateUploaded":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.DateUploaded) : query.OrderByDescending(s => s.DateUploaded);
                        break;

                    default:
                        query = query.OrderBy(s => s.DocumentName);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<DiagnosticResultModel>()
                {
                    TotalCount = count,
                    Result = items.ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<DiagnosticResultModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<DiagnosticResultModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long DiagnosticResultId)
        {
            try
            {

                if (DiagnosticResultId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_DiagnosticResults.Find(DiagnosticResultId);

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                result.Deleted = true;


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Diagnostic Result: DocumentName = {result.DocumentName}, DocumentExtension = {result.DocumentExtension}, DateUploaded = {result.DateUploaded} ";
                await auditTrail.SaveAuditTrail(details, "Diagnostic Result", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model)
        {
            try
            {
                if (model.targetIds.Count <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.NHS_DiagnosticResults.FindAsync(item);
                    if (data != null)
                    {
                        data.Deleted = true;
                    }
                };

                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Records Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Multiple Diagnostic Result: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Diagnostic Result", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}