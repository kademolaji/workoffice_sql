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
    public class PatientDocumentService : IPatientDocumentService
    {

        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public PatientDocumentService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(PatientDocumentModel model)
        {
            try
            {

                if (model.PatientDocumentId > 0)
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
                NHS_Patientdocument entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Patientdocument
                        {
                            DocumentTypeId = model.DocumentTypeId,
                            PatientId = model.PatientId,
                            PhysicalLocation = model.PhysicalLocation,
                            DocumentName = model.DocumentName,
                            DocumentExtension = model.DocumentExtension,
                            DocumentFile = model.DocumentFile,
                            ClinicDate = model.ClinicDate,
                            DateUploaded = model.DateUploaded,
                            SpecialtyId = model.SpecialityId,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Patientdocuments.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Patient Document: DocumentName = {model.DocumentName}, DocumentExtension = {model.DocumentExtension}, PatientDocumentId = {model.PatientDocumentId} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Document", "Update");
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
                    Id = entity.PatientDocumentId,
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

        public async Task<ApiResponse<CreateResponse>> Update(PatientDocumentModel model)
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

                var existingNHSNumber = context.NHS_Patientdocuments.Any(x => x.DocumentName == model.DocumentName && x.PatientDocumentId != model.PatientDocumentId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another document already exists with the given document name" }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Patientdocuments.FindAsync(model.PatientDocumentId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.DocumentTypeId = model.DocumentTypeId;
                        entity.PatientId = model.PatientId;
                        entity.PhysicalLocation = model.PhysicalLocation;
                        entity.DocumentName = model.DocumentName;
                        entity.DocumentExtension = model.DocumentExtension;
                        entity.DocumentFile = model.DocumentFile;
                        entity.ClinicDate = model.ClinicDate;
                        entity.DateUploaded = model.DateUploaded;
                        entity.SpecialtyId = model.SpecialityId;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Patient Document: DocumentName = {model.DocumentName}, DocumentExtension = {model.DocumentExtension}, DateUploaded = {model.DateUploaded} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Document", "Update");
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

        public async Task<ApiResponse<GetResponse<PatientDocumentModel>>> Get(long patientDocumentId)
        {

            try
            {
                var apiResponse = new ApiResponse<GetResponse<PatientDocumentModel>>();


                var result = await (from doc in context.NHS_Patientdocuments
                                    where doc.PatientDocumentId == patientDocumentId
                                    select new PatientDocumentModel
                                    {
                                        PatientDocumentId = doc.PatientDocumentId,
                                        DocumentTypeId = doc.DocumentTypeId,
                                        PatientId = doc.PatientId,
                                        PhysicalLocation = doc.PhysicalLocation,
                                        DocumentName = doc.DocumentName,
                                        DocumentExtension = doc.DocumentExtension,
                                        DocumentFile = doc.DocumentFile,
                                        ClinicDate = doc.ClinicDate,
                                        DateUploaded = doc.DateUploaded,
                                        SpecialityId = doc.SpecialtyId,

                                    }).FirstOrDefaultAsync();


                var response = new GetResponse<PatientDocumentModel>()
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
                return new ApiResponse<GetResponse<PatientDocumentModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientDocumentModel>() { Status = false, Message = ex.Message }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<SearchReply<PatientDocumentModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "documentName" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<PatientDocumentModel>>();


                IQueryable<PatientDocumentModel> query = (from doc in context.NHS_Patientdocuments
                                                          where doc.PatientId == options.Parameter.Id
                                                          select new PatientDocumentModel
                                                          {
                                                              PatientDocumentId = doc.PatientDocumentId,
                                                              DocumentTypeId = doc.DocumentTypeId,
                                                              PatientId = doc.PatientId,
                                                              PhysicalLocation = doc.PhysicalLocation,
                                                              DocumentName = doc.DocumentName,
                                                              DocumentExtension = doc.DocumentExtension,
                                                              DocumentFile = doc.DocumentFile,
                                                              ClinicDate = doc.ClinicDate,
                                                              DateUploaded = doc.DateUploaded,
                                                              SpecialityId = doc.SpecialtyId,
                                                              Speciality = doc.SpecialtyId != null ? context.Specialties.FirstOrDefault(x => x.SpecialtyId == doc.SpecialtyId).Name : "",
                                                              ConsultantName = "",

                                                          }).AsQueryable();

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.DocumentName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.PhysicalLocation.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Speciality.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                   );
                }
                int offset = (pageNumber) * pageSize;


                switch (sortField)
                {
                    case "documentName":
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


                var response = new SearchReply<PatientDocumentModel>()
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
                return new ApiResponse<SearchReply<PatientDocumentModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<PatientDocumentModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long patientDocumentId)
        {
            try
            {

                if (patientDocumentId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Patientdocuments.Find(patientDocumentId);

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

                var details = $"Deleted Patient Document: DocumentName = {result.DocumentName}, DocumentExtension = {result.DocumentExtension}, DateUploaded = {result.DateUploaded} ";
                await auditTrail.SaveAuditTrail(details, "Patient Document", "Delete");

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
                    var data = await context.NHS_Patientdocuments.FindAsync(item);
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

                var details = $"Deleted Multiple Patient Document: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Patient Document", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}