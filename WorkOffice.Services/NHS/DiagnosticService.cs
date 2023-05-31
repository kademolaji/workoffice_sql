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
    public class DiagnosticService : IDiagnosticService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public DiagnosticService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(DiagnosticModel model)
        {
            try
            {

                if (model.DiagnosticId > 0)
                {
                    return await Update(model);
                }
                if (string.IsNullOrEmpty(model.Problem))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Problem is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Diagnostic entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Diagnostic
                        {
                            PatientId = model.PatientId,
                            SpecialtyId = model.SpecialtyId,
                            Problem = model.Problem,
                            DTD = model.DTD,
                            Status = model.Status,
                            ConsultantName = model.ConsultantName,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Diagnostics.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Diagnostic Information: ConsultantName = {model.ConsultantName},with Problem = {model.Problem} ";
                            await auditTrail.SaveAuditTrail(details, "Diagnostic Information", "Update");
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
                    Id = entity.DiagnosticId,
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

        public async Task<ApiResponse<CreateResponse>> Update(DiagnosticModel model)
        {
            try
            {


                if (string.IsNullOrEmpty(model.Problem))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Problem is required." }, IsSuccess = false };
                }

                var existingNHSNumber = context.NHS_Diagnostics.Any(x => x.DiagnosticId != model.DiagnosticId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another Diagnostic already exists" }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Diagnostics.FindAsync(model.DiagnosticId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.PatientId = model.PatientId;
                        entity.SpecialtyId = model.SpecialtyId;
                        entity.Problem = model.Problem;
                        entity.DTD = model.DTD;
                        entity.Status = model.Status;
                        entity.ConsultantName = model.ConsultantName;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Diagnostic Information: ConsultantName = {model.ConsultantName},with Problem = {model.Problem} ";
                            await auditTrail.SaveAuditTrail(details, "Diagnostic Information", "Update");
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
                    Id = entity.DiagnosticId,
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

        public async Task<ApiResponse<SearchReply<DiagnosticModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "problem" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<DiagnosticModel>>();


             //   IQueryable<NHS_Diagnostic> query = context.NHS_Diagnostics;

                IQueryable<DiagnosticModel> query = (from app in context.NHS_Diagnostics
                                                              join pat in context.NHS_Patients on app.PatientId equals pat.PatientId
                                                              select new DiagnosticModel
                                                              {
                                                                  DiagnosticId = app.DiagnosticId,
                                                                  PatientId = app.PatientId,
                                                                  PatientName = pat.FirstName + " " + pat.LastName,
                                                                  SpecialtyId = app.SpecialtyId,
                                                                  Problem = app.Problem,
                                                                  DTD = app.DTD,
                                                                  ConsultantName = app.ConsultantName,
                                                                  Status = app.Status,
                                                                  Specialty = context.Specialties.FirstOrDefault(x => x.SpecialtyId == app.SpecialtyId).Name
                                                              }).AsQueryable();
                int offset = (pageNumber) * pageSize;
                if (options.Parameter.Id > 0)
                {
                    query = query.Where(x => x.PatientId == options.Parameter.Id);
                }

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.Problem.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Status.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.PatientName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Specialty.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.ConsultantName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                    
                }
                switch (sortField)
                {
                    case "problem":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Problem) : query.OrderByDescending(s => s.Problem);
                        break;
                    case "consultantname":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.ConsultantName) : query.OrderByDescending(s => s.ConsultantName);
                        break;
                    case "patientName":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientName) : query.OrderByDescending(s => s.PatientName);
                        break;
                    case "specialty":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Specialty) : query.OrderByDescending(s => s.Specialty);
                        break;
                    default:
                        query = query.OrderBy(s => s.Problem);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<DiagnosticModel>()
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
                return new ApiResponse<SearchReply<DiagnosticModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<DiagnosticModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<DiagnosticModel>>> Get(long DiagnosticId)
        {
            try
            {
                if (DiagnosticId <= 0)
                {
                    return new ApiResponse<GetResponse<DiagnosticModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<DiagnosticModel> { Status = false, Entity = null, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<DiagnosticModel>>();

                var result = await context.NHS_Diagnostics.FirstOrDefaultAsync(x => x.DiagnosticId == DiagnosticId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<DiagnosticModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<DiagnosticModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<DiagnosticModel>()
                {
                    Status = true,
                    Entity =  new DiagnosticModel {
                        DiagnosticId = result.DiagnosticId,
                        PatientId = result.PatientId,
                        SpecialtyId = result.SpecialtyId,
                        Problem = result.Problem,
                        DTD = result.DTD,
                        ConsultantName = result.ConsultantName,
                        Status = result.Status,
                    },
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<DiagnosticModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<DiagnosticModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long DiagnosticId)
        {
            try
            {

                if (DiagnosticId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "DiagnosticId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Diagnostics.Find(DiagnosticId);

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

                var details = $"Deleted Diagnostic Information:";
                await auditTrail.SaveAuditTrail(details, "Diagnostic Information", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "DiagnosticId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.NHS_Diagnostics.FindAsync(item);
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

                var details = $"Deleted Multiple Diagnostic Information: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Diagnostic Information", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}