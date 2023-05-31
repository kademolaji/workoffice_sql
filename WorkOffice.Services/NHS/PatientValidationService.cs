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
    public class PatientValidationService : IPatientValidationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public PatientValidationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(PatientValidationModel model)
        {
            try
            {

                if (model.PatientValidationId > 0)
                {
                    return await Update(model);
                }
                if (model.PatientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Patient is required." }, IsSuccess = false };
                }
                var patientDetail = context.NHS_Patients.Where(a => a.PatientId == model.PatientId).FirstOrDefault();
                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Patient_Validation entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Patient_Validation
                        {
                            DistrictNumber = patientDetail.DistrictNumber,
                            PathWayCondition = model.PathWayCondition,
                            PathWayEndDate = model.PathWayEndDate,
                            PathWayNumber = model.PathWayNumber,
                            PathWayStartDate = model.PathWayStartDate,
                            PathWayStatusId = model.PathWayStatusId,
                            PatientId = model.PatientId,
                            RTTId = model.RTTId,
                            SpecialtyId = model.SpecialtyId,
                            RTTWait = model.RTTWait,
                            NHSNumber = model.NHSNumber,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Patient_Validations.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Patient Pathway: DistrictNumber = {model.DistrictNumber} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Pathway", "Added");
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

        public async Task<ApiResponse<CreateResponse>> Update(PatientValidationModel model)
        {
            try
            {

                var existingNHSNumber = context.NHS_Patient_Validations.Any(x => x.PatientValidationId == model.PatientValidationId && x.PatientId != model.PatientId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another Patient pathway already exists" }, IsSuccess = false };
                }

                var patientDetail = context.NHS_Patients.Where(a => a.PatientId == model.PatientId).FirstOrDefault();
                var pathwayDetail = context.NHS_Patient_Validations.Where(a => a.PatientValidationId == model.PatientValidationId).FirstOrDefault();


                if (string.IsNullOrEmpty(patientDetail.NHSNumber))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "NHS Number is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(patientDetail.DistrictNumber))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "District Number is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(pathwayDetail.PathWayNumber))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Pathway Number is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Patient_Validations.FindAsync(model.PatientValidationId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }

                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.DistrictNumber = patientDetail.DistrictNumber;
                        entity.PathWayCondition = model.PathWayCondition;
                        entity.PathWayEndDate = model.PathWayEndDate;
                        entity.PathWayNumber = pathwayDetail.PathWayNumber;
                        entity.PathWayStartDate = model.PathWayStartDate;
                        entity.PathWayStatusId = model.PathWayStatusId;
                        entity.PatientId = model.PatientId;
                        entity.RTTId = model.RTTId;
                        entity.SpecialtyId = model.SpecialtyId;
                        entity.RTTWait = model.RTTWait;
                        entity.NHSNumber = patientDetail.NHSNumber;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Patient Pathway: DistrictNumber = {patientDetail.DistrictNumber} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Pathway", "Update");
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

        public async Task<ApiResponse<SearchReply<PatientValidationModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "districtNumber" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<PatientValidationModel>>();


                //IQueryable<NHS_Patient_Validation> query = context.NHS_Patient_Validations;


                IQueryable<PatientValidationModel> query = (from x in context.NHS_Patient_Validations
                                                            join y in context.Specialties on x.SpecialtyId equals y.SpecialtyId
                                                            where x.Deleted == false
                                                            select new PatientValidationModel
                                                            {
                                                                PatientId = x.PatientId,
                                                                DistrictNumber = x.DistrictNumber,
                                                                PathWayCondition = x.PathWayCondition,
                                                                PathWayEndDate = x.PathWayEndDate,
                                                                PathWayNumber = x.PathWayNumber,
                                                                PathWayStartDate = x.PathWayStartDate,
                                                                PathWayStatusId = x.PathWayStatusId,
                                                                RTTId = x.RTTId,
                                                                SpecialtyId = x.SpecialtyId,
                                                                RTTWait = x.RTTWait,
                                                                PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Name : null,
                                                                PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Code : null,
                                                                SpecialityCode = y.Code,
                                                                SpecialityName = y.Name,
                                                                NHSNumber = x.NHSNumber,
                                                                PatientValidationId = x.PatientValidationId
                                                            }).AsQueryable();
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.DistrictNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.NHSNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.PathWayNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "pathWayNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PathWayNumber) : query.OrderByDescending(s => s.PathWayNumber);
                        break;
                    case "nhsNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.NHSNumber) : query.OrderByDescending(s => s.NHSNumber);
                        break;
                    case "districtNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.DistrictNumber) : query.OrderByDescending(s => s.DistrictNumber);
                        break;

                    default:
                        query = query.OrderBy(s => s.PathWayNumber);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();

                var response = new SearchReply<PatientValidationModel>()
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
                return new ApiResponse<SearchReply<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<PatientValidationModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<PatientValidationModel>>> Get(long PatientValidationId)
        {
            try
            {
                if (PatientValidationId <= 0)
                {
                    return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationModel> { Status = false, Entity = null, Message = "PatientValidationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<PatientValidationModel>>();


                var result = await (from x in context.NHS_Patient_Validations
                                    join p in context.NHS_Patients on x.PatientId equals p.PatientId
                                    join y in context.Specialties on x.SpecialtyId equals y.SpecialtyId
                                    where x.PatientValidationId == PatientValidationId
                                    select new PatientValidationModel
                                    {
                                        PatientValidationId = x.PatientValidationId,
                                        PatientId = x.PatientId,
                                        DistrictNumber = x.DistrictNumber,
                                        PathWayCondition = x.PathWayCondition,
                                        PathWayEndDate = x.PathWayEndDate,
                                        PathWayNumber = x.PathWayNumber,
                                        PathWayStartDate = x.PathWayStartDate,
                                        PathWayStatusId = x.PathWayStatusId,
                                        RTTId = x.RTTId,
                                        SpecialtyId = x.SpecialtyId,
                                        RTTWait = x.RTTWait,
                                        PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Name : null,
                                        PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Code : null,
                                        SpecialityCode = y.Code,
                                        SpecialityName = y.Name,
                                        NHSNumber = x.NHSNumber,
                                        PatientName = p.DistrictNumber + " - " + p.FirstName + " " + p.MiddleName + " " + p.LastName
                                    }).FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<PatientValidationModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<PatientValidationModel>()
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
                return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<PatientValidationModel>>>> GetPathwayByPatientId(long patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return new ApiResponse<GetResponse<List<PatientValidationModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<PatientValidationModel>> { Status = false, Entity = null, Message = "Patient is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<List<PatientValidationModel>>>();


                var result = await (from x in context.NHS_Patient_Validations
                                    join p in context.NHS_Patients on x.PatientId equals p.PatientId
                                    join y in context.Specialties on x.SpecialtyId equals y.SpecialtyId
                                    where x.PatientId == patientId
                                    select new PatientValidationModel
                                    {
                                        PatientValidationId = x.PatientValidationId,
                                        PatientId = x.PatientId,
                                        DistrictNumber = x.DistrictNumber,
                                        PathWayCondition = x.PathWayCondition,
                                        PathWayEndDate = x.PathWayEndDate,
                                        PathWayNumber = x.PathWayNumber,
                                        PathWayStartDate = x.PathWayStartDate,
                                        PathWayStatusId = x.PathWayStatusId,
                                        RTTId = x.RTTId,
                                        SpecialtyId = x.SpecialtyId,
                                        RTTWait = x.RTTWait,
                                        PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Name : null,
                                        PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Code : null,
                                        SpecialityCode = y.Code,
                                        SpecialityName = y.Name,
                                        NHSNumber = x.NHSNumber,
                                        PatientName = p.DistrictNumber + " - " + p.FirstName + " " + p.MiddleName + " " + p.LastName
                                    }).ToListAsync();

                if (!result.Any())
                {
                    return new ApiResponse<GetResponse<List<PatientValidationModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<PatientValidationModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                var response = new GetResponse<List<PatientValidationModel>>()
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
                return new ApiResponse<GetResponse<List<PatientValidationModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<PatientValidationModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<PatientValidationModel>>> GetSinglePatientValidation(long PatientId)
        {
            try
            {
                if (PatientId <= 0)
                {
                    return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationModel> { Status = false, Entity = null, Message = "PatientId is required." }, IsSuccess = false };
                }

                var distNumber = context.NHS_Patients.Where(a => a.PatientId == PatientId).FirstOrDefault();
                var valPathWay = context.NHS_Patient_Validations.Where(a => a.NHSNumber == distNumber.NHSNumber).FirstOrDefault().PatientValidationId;
                var patDetPathWay = context.NHS_Patient_Validation_Details.Where(a => a.PatientValidationId == valPathWay).FirstOrDefault();
                int? valDetPathWay;
                if (patDetPathWay != null)
                {
                    valDetPathWay = context.NHS_Patient_Validation_Details.Where(a => a.PatientValidationId == valPathWay).OrderByDescending(x => x.PatientValidationDetailsId).FirstOrDefault().PathWayStatusId;
                }
                else
                {
                    valDetPathWay = null;
                }

                var apiResponse = new ApiResponse<GetResponse<PatientValidationModel>>();

                //var result = await context.NHS_Patient_Validations.FirstOrDefaultAsync(x => x.PatientId == PatientId);

                var result = await (from x in context.NHS_Patient_Validations
                                    join y in context.Specialties on x.SpecialtyId equals y.SpecialtyId
                                    where x.NHSNumber == distNumber.NHSNumber && x.Deleted == false
                                    select new PatientValidationModel
                                    {
                                        PatientId = x.PatientId,
                                        DistrictNumber = x.DistrictNumber,
                                        PathWayCondition = x.PathWayCondition,
                                        PathWayEndDate = x.PathWayEndDate,
                                        PathWayNumber = x.PathWayNumber,
                                        PathWayStartDate = x.PathWayStartDate,
                                        PathWayStatusId = x.PathWayStatusId,
                                        RTTId = x.RTTId,
                                        SpecialtyId = x.SpecialtyId,
                                        RTTWait = x.RTTWait,
                                        PathWayStatusCode = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Name : null,
                                        PathWayStatusName = x.PathWayStatusId != null ? context.PathWayStatuses.Where(a => a.PathWayStatusId == a.PathWayStatusId).FirstOrDefault().Code : null,
                                        SpecialityCode = y.Code,
                                        SpecialityName = y.Name,
                                        NHSNumber = x.NHSNumber,
                                    }).FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<PatientValidationModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<PatientValidationModel>()
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
                return new ApiResponse<GetResponse<PatientValidationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientValidationModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<DeleteReply>> Delete(long PatientValidationId)
        {
            try
            {

                if (PatientValidationId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientValidationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Patient_Validations.Find(PatientValidationId);

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

                var details = $"Deleted Patient Validation";
                await auditTrail.SaveAuditTrail(details, "Patient Validation", "Delete");

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
                    var data = await context.NHS_Patient_Validations.FindAsync(item);
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

                var details = $"Deleted Multiple Patient Validation: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Patient Validation", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> PatientDetailsValidation(long PatientValidationDetailsId)
        {
            try
            {

                if (PatientValidationDetailsId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientValidationDetailsId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Patient_Validation_Details.Where(x => x.PatientValidationDetailsId == PatientValidationDetailsId).FirstOrDefault();
                int patientValId = result.PatientValidationId;

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                context.NHS_Patient_Validation_Details.Remove(result);
                //result.Deleted = true;


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

                    //UpdatePatientValidationStatus(PatientValidation.PatientValidationDetailsId);
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
    }
}