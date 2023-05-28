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
    public class WaitingListService : IWaitingListService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public WaitingListService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(WaitingListModel model)
        {
            try
            {

                if (model.WaitinglistId > 0)
                {
                    return await Update(model);
                }

                if (model.WaitTypeId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "WaitType is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Waitinglist entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Waitinglist
                        {
                            WaitTypeId = model.WaitTypeId,
                            SpecialtyId = model.SpecialityId,
                            TCIDate = model.TCIDate,
                            WaitinglistDate = model.WaitinglistDate,
                            WaitinglistTime = model.WaitinglistTime,
                            PatientId = model.PatientId,
                            patientValidationId = model.patientValidationId,
                            Condition = model.Condition,
                            WaitinglistStatus = model.WaitinglistStatus,
                            Active = true,
                            CreatedBy = model.CurrentUsername,
                            CreatedOn = DateTime.UtcNow,
                            Deleted = false
                        };

                        context.NHS_Waitinglists.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Waitinglist: WaitTypeId = {model.WaitTypeId}, Condition = {model.Condition}, WaitinglistTime = {model.WaitinglistTime} ";
                            await auditTrail.SaveAuditTrail(details, "Waitinglist", "Create");
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
                    Id = entity.WaitinglistId,
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

        public async Task<ApiResponse<CreateResponse>> Update(WaitingListModel model)
        {
            try
            {


                if (model.WaitTypeId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "WaitType is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Waitinglists.FindAsync(model.WaitinglistId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.WaitTypeId = model.WaitTypeId;
                        entity.SpecialtyId = model.SpecialityId;
                        entity.TCIDate = model.TCIDate;
                        entity.WaitinglistDate = model.WaitinglistDate;
                        entity.WaitinglistTime = model.WaitinglistTime;
                        entity.PatientId = model.PatientId;
                        entity.Condition = model.Condition;
                        entity.WaitinglistStatus = model.WaitinglistStatus;
                        entity.patientValidationId = model.patientValidationId;
                        entity.UpdatedBy = model.CurrentUsername;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Waitinglist: WaitTypeId = {model.WaitTypeId}, Condition = {model.Condition}, WaitinglistTime = {model.WaitinglistTime} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Information", "Update");
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
                    Id = entity.WaitinglistId,
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

        public async Task<ApiResponse<SearchReply<WaitingListModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "condition" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<WaitingListModel>>();


                IQueryable<NHS_Waitinglist> query = context.NHS_Waitinglists;
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.Condition.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                   );
                }
                switch (sortField)
                {
                    case "condition":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Condition) : query.OrderByDescending(s => s.Condition);
                        break;

                    default:
                        query = query.OrderBy(s => s.Condition);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<WaitingListModel>()
                {
                    TotalCount = count,
                    Result = items.Select(model => new WaitingListModel
                    {
                        WaitinglistId = model.WaitinglistId,
                        WaitTypeId = model.WaitTypeId,
                        SpecialityId = model.SpecialtyId,
                        TCIDate = model.TCIDate,
                        WaitinglistDate = model.WaitinglistDate,
                        WaitinglistTime = model.WaitinglistTime,
                        PatientId = model.PatientId,
                        Condition = model.Condition,
                        WaitinglistStatus = model.WaitinglistStatus,
                        patientValidationId = model.patientValidationId,
                        DistrictNumber = (from a in context.NHS_Patients where a.PatientId == model.PatientId select a.DistrictNumber).FirstOrDefault(),
                        PathWayNumber = (from a in context.NHS_Patient_Validations
                                         join b in context.NHS_Patients on a.PatientId equals b.PatientId
                                         where a.PatientValidationId == model.patientValidationId
                                         select a.PathWayNumber
                                       ).FirstOrDefault()

                    }).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<WaitingListModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<WaitingListModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<WaitingListModel>>> Get(long waitinglistId)
        {
            try
            {
                if (waitinglistId <= 0)
                {
                    return new ApiResponse<GetResponse<WaitingListModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<WaitingListModel> { Status = false, Entity = null, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<WaitingListModel>>();

                var result = await context.NHS_Waitinglists.FirstOrDefaultAsync(x => x.WaitinglistId == waitinglistId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<WaitingListModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<WaitingListModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<WaitingListModel>()
                {
                    Status = true,
                    Entity = new WaitingListModel
                    {
                        WaitinglistId = result.WaitinglistId,
                        WaitTypeId = result.WaitTypeId,
                        SpecialityId = result.SpecialtyId,
                        TCIDate = result.TCIDate,
                        WaitinglistDate = result.WaitinglistDate,
                        WaitinglistTime = result.WaitinglistTime,
                        PatientId = result.PatientId,
                        Condition = result.Condition,
                        WaitinglistStatus = result.WaitinglistStatus,
                        patientValidationId = result.patientValidationId,
                        DistrictNumber = (from a in context.NHS_Patients where a.PatientId == result.PatientId select a.DistrictNumber + " - " + a.FirstName + " " + a.MiddleName + " " + a.LastName).FirstOrDefault(),
                        PathWayNumber = (from a in context.NHS_Patient_Validations join b in context.NHS_Patients on a.PatientId equals b.PatientId
                                         where a.PatientValidationId == result.patientValidationId
                                         select a.PathWayNumber + " - " + b.FirstName + " " + b.MiddleName + " " + b.LastName
                                       ).FirstOrDefault()
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
                return new ApiResponse<GetResponse<WaitingListModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<WaitingListModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long waitinglistId)
        {
            try
            {

                if (waitinglistId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Waitinglists.Find(waitinglistId);

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

                var details = $"Deleted Waitinglist: Condition = {result.Condition}, WaitinglistTime = {result.WaitinglistTime}, WaitinglistId = {result.WaitinglistId} ";
                await auditTrail.SaveAuditTrail(details, "Waitinglist", "Delete");

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
                    var data = await context.NHS_Waitinglists.FindAsync(item);
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

                var details = $"Deleted Multiple Waitinglist: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Waitinglist", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}