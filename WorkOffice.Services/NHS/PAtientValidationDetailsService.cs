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
    public class PatientValidationDetailsService : IPatientValidationDetailsService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public PatientValidationDetailsService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(PatientValidationDetailsModel model)
        {
            try
            {

                if (model.PatientValidationDetailsId > 0)
                {
                    return await Update(model);
                }
                if (model.PatientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Patient is required." }, IsSuccess = false };
                }
                if (model.PathWayStatusId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "PathWayStatus is required." }, IsSuccess = false };
                }
                if (model.ConsultantId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Consultant is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }
                var status = context.PathWayStatuses.FirstOrDefault(x => x.PathWayStatusId == model.PathWayStatusId);
                if (status.AllowClosed && string.IsNullOrEmpty(model.EndDate))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "End date is required for the selected status." }, IsSuccess = false };
                }
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;

                NHS_Patient_Validation_Detail entity = null;

                DateTime? endDate = null;
                if (!string.IsNullOrEmpty(model.EndDate))
                {
                    endDate = Convert.ToDateTime(model.EndDate);
                }

                using (var trans = context.Database.BeginTransaction())
                {

                    try
                    {
                        entity = new NHS_Patient_Validation_Detail
                        {
                            PatientValidationId = model.PatientValidationId,
                            PathWayStatusId = model.PathWayStatusId,
                            SpecialtyId = model.SpecialityId,
                            Date = model.Date,
                            ConsultantId = model.ConsultantId,
                            EndDate = endDate,
                            PatientId = model.PatientId,
                            Activity = model.Activity,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Patient_Validation_Details.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            UpdatePatientValidation(entity.PatientValidationDetailsId);

                            var details = $"Created New Patient Validation Details: PatientId = {model.PatientId} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Validation Details", "Added");
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

        public async Task<ApiResponse<CreateResponse>> Update(PatientValidationDetailsModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;

                if (model.PatientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Patient is required." }, IsSuccess = false };
                }
                if (model.PathWayStatusId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "PathWayStatus is required." }, IsSuccess = false };
                }
                if (model.ConsultantId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Consultant is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }
                var status = context.PathWayStatuses.FirstOrDefault(x => x.PathWayStatusId == model.PathWayStatusId);
                if (status.AllowClosed && string.IsNullOrEmpty(model.EndDate))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "End date is required for the selected status." }, IsSuccess = false };
                }
                var entity = await context.NHS_Patient_Validation_Details.FindAsync(model.PatientValidationDetailsId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }

                DateTime? endDate = null;
                if (!string.IsNullOrEmpty(model.EndDate))
                {
                    endDate = Convert.ToDateTime(model.EndDate);
                }

                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.PatientValidationId = model.PatientValidationId;
                        entity.PathWayStatusId = model.PathWayStatusId;
                        entity.SpecialtyId = model.SpecialityId;
                        entity.Date = model.Date;
                        entity.ConsultantId = model.ConsultantId;
                        entity.EndDate = endDate;
                        entity.PatientId = model.PatientId;
                        entity.Activity = model.Activity;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            UpdatePatientValidation(entity.PatientValidationDetailsId);

                            var details = $"Updated Patient Validation Details: PatientId = {model.PatientId} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Validation Details", "Update");
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

        public async Task<ApiResponse<CreateResponse>> Merge(MergePathwayModel model)
        {
            try
            {


                if (model.PatientValidationDetailsId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "PatientValidationDetailsId is required." }, IsSuccess = false };
                }
                if (model.PatientValidationId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "PathWay is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Patient_Validation_Details.FindAsync(model.PatientValidationDetailsId);

                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                var oldPatientValidationId = entity.PatientValidationId;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.PatientValidationId = model.PatientValidationId;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {

                            var PatientValidation = context.NHS_Patient_Validation_Details.Where(x => x.PatientValidationId == oldPatientValidationId && x.Deleted == false).OrderByDescending(x => x.PatientValidationDetailsId).FirstOrDefault();
                            if (PatientValidation != null)
                            {
                                UpdatePatientValidation(PatientValidation.PatientValidationDetailsId);
                            }
                            var details = $"Merge Pathway: PatientValidationId = {model.PatientValidationId} ";
                            await auditTrail.SaveAuditTrail(details, "Merged Pathway", "Added");
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
        public async Task<ApiResponse<SearchReply<PatientValidationDetailsModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "activity" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<PatientValidationDetailsModel>>();

                IQueryable<PatientValidationDetailsModel> query = (from x in context.NHS_Patient_Validation_Details
                                                                   where x.PatientValidationId == options.Parameter.Id && x.Deleted == false
                                                                   select new PatientValidationDetailsModel
                                                                   {
                                                                       PatientValidationDetailsId = x.PatientValidationDetailsId,
                                                                       PatientValidationId = x.PatientValidationId,
                                                                       PathWayStatusId = x.PathWayStatusId,
                                                                       SpecialityId = x.SpecialtyId,
                                                                       Date = x.Date,
                                                                       ConsultantId = x.ConsultantId,
                                                                       EndDate = x.EndDate.ToString(),
                                                                       PatientId = x.PatientId,
                                                                       Activity = x.Activity,
                                                                       ConsultantCode = x.ConsultantId != null ? context.Consultants.Where(a => a.ConsultantId == x.ConsultantId).FirstOrDefault().Name : null,
                                                                       ConsultantName = x.ConsultantId != null ? context.Consultants.Where(a => a.ConsultantId == x.ConsultantId).FirstOrDefault().Name : null,
                                                                       PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == x.PathWayStatusId).FirstOrDefault().Name : null,
                                                                       PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == x.PathWayStatusId).FirstOrDefault().Code : null,
                                                                       SpecialityCode = x.SpecialtyId != null ? context.Specialties.Where(a => a.SpecialtyId == x.SpecialtyId).FirstOrDefault().Name : null,
                                                                       SpecialityName = x.SpecialtyId != null ? context.Specialties.Where(a => a.SpecialtyId == x.SpecialtyId).FirstOrDefault().Name : null,
                                                                   }).AsQueryable();
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.ConsultantName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.PathWayStatusName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.SpecialityName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "activity":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Activity) : query.OrderByDescending(s => s.Activity);
                        break;
                    case "endDate":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.EndDate) : query.OrderByDescending(s => s.EndDate);
                        break;
                    case "startDate":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Date) : query.OrderByDescending(s => s.Date);
                        break;

                    default:
                        query = query.OrderBy(s => s.Date).ThenBy(d => d.PathWayStatusCode);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<PatientValidationDetailsModel>()
                {
                    TotalCount = count,
                    Result = items.OrderBy(x=>x.SpecialityName).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<PatientValidationDetailsModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<PatientValidationDetailsModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<PatientValidationDetailsModel>>> Get(long patientValidationDetailsId)
        {
            try
            {
                if (patientValidationDetailsId <= 0)
                {
                    return new ApiResponse<GetResponse<PatientValidationDetailsModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationDetailsModel> { Status = false, Entity = null, Message = "PatientValidationDetailsId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<PatientValidationDetailsModel>>();


                var result = await (from x in context.NHS_Patient_Validation_Details
                                    where x.PatientValidationDetailsId == patientValidationDetailsId
                                    select new PatientValidationDetailsModel
                                    {
                                        PatientValidationDetailsId = x.PatientValidationDetailsId,
                                        PatientValidationId = x.PatientValidationId,
                                        PathWayStatusId = x.PathWayStatusId,
                                        SpecialityId = x.SpecialtyId,
                                        Date = x.Date,
                                        ConsultantId = x.ConsultantId,
                                        EndDate = x.EndDate.ToString(),
                                        PatientId = x.PatientId,
                                        Activity = x.Activity,
                                        ConsultantCode = x.ConsultantId != null ? context.Consultants.Where(a => a.ConsultantId == x.ConsultantId).FirstOrDefault().Name : null,
                                        ConsultantName = x.ConsultantId != null ? context.Consultants.Where(a => a.ConsultantId == x.ConsultantId).FirstOrDefault().Name : null,
                                        PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == x.PathWayStatusId).FirstOrDefault().Name : null,
                                        PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == x.PathWayStatusId).FirstOrDefault().Code : null,
                                        SpecialityCode = x.SpecialtyId != null ? context.Specialties.Where(a => a.SpecialtyId == x.SpecialtyId).FirstOrDefault().Name : null,
                                        SpecialityName = x.SpecialtyId != null ? context.Specialties.Where(a => a.SpecialtyId == x.SpecialtyId).FirstOrDefault().Name : null,
                                    }).FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ApiResponse<GetResponse<PatientValidationDetailsModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<PatientValidationDetailsModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<PatientValidationDetailsModel>()
                {
                    Status = true,
                    Entity = result,
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<PatientValidationDetailsModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationDetailsModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        public async Task<ApiResponse<DeleteReply>> DeletePatientDetailsValidation(long patientValidationDetailsId)
        {
            try
            {

                if (patientValidationDetailsId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientValidationDetailsId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Patient_Validation_Details.Where(x => x.PatientValidationDetailsId == patientValidationDetailsId).FirstOrDefault();


                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                int patientValId = result.PatientValidationId;

                context.NHS_Patient_Validation_Details.Remove(result);


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                if (response.Status == true)
                {
                    var PatientValidation = context.NHS_Patient_Validation_Details.Where(x => x.PatientValidationId == patientValId && x.Deleted == false).OrderByDescending(x => x.PatientValidationDetailsId).FirstOrDefault();
                    if (PatientValidation != null)
                    {
                        UpdatePatientValidation(PatientValidation.PatientValidationDetailsId);
                    }
                }
                //return response;

                var details = $"Deleted Patient Validation:";
                await auditTrail.SaveAuditTrail(details, "Patient Validation", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        private void UpdatePatientValidation(long patientValidationDetailsId)
        {
            var patientValidationDetails = context.NHS_Patient_Validation_Details.Where(x => x.PatientValidationDetailsId == patientValidationDetailsId && x.Deleted == false).FirstOrDefault();
            if (patientValidationDetails != null)
            {
                var patient_Validation = context.NHS_Patient_Validations.Where(x => x.PatientValidationId == patientValidationDetails.PatientValidationId).FirstOrDefault();
                var status = context.PathWayStatuses.FirstOrDefault(x => x.PathWayStatusId == patientValidationDetails.PathWayStatusId);
                if (patient_Validation != null)
                {
                    patient_Validation.PathWayStatusId = patientValidationDetails.PathWayStatusId;
                    patient_Validation.PathWayEndDate = patientValidationDetails.EndDate;
                    if (status != null && status.AllowClosed)
                    {
                        patient_Validation.RTTId = 1;
                    }
                    else
                    {
                        patient_Validation.RTTId = 2;
                    }
                    context.SaveChanges();
                }
            }

        }
    }
}