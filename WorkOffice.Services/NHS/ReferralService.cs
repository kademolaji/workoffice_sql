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
    public class ReferralService : IReferralService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public ReferralService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(ReferralModel model)
        {
            try
            {

                if (model.ReferralId > 0)
                {
                    return await Update(model);
                }
                if (string.IsNullOrEmpty(model.ConsultantName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "ConsultantName is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Referral entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Referral
                        {
                            PatientId = model.PatientId,
                            SpecialtyId = model.SpecialtyId,
                            ConsultantId = model.ConsultantId,
                            ConsultantName = model.ConsultantName,
                            DocumentExtension = model.DocumentExtension,
                            DocumentFile = model.DocumentFile,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Referrals.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Referral Information: ";
                            await auditTrail.SaveAuditTrail(details, "Referral Information", "Update");
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
                    Id = entity.ReferralId,
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

        public async Task<ApiResponse<CreateResponse>> Update(ReferralModel model)
        {
            try
            {


                if (string.IsNullOrEmpty(model.ConsultantName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "ConsultantName is required." }, IsSuccess = false };
                }


                var existingNHSNumber = context.NHS_Referrals.Any(x => x.ReferralId != model.ReferralId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another Referral already exists with the given NHS Number" }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Referrals.FindAsync(model.ReferralId);
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
                        entity.ConsultantId = model.ConsultantId;
                        entity.ConsultantName = model.ConsultantName;
                        entity.DocumentExtension = model.DocumentExtension;
                        entity.DocumentFile = model.DocumentFile;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Referral Information:";
                            await auditTrail.SaveAuditTrail(details, "Referral Information", "Update");
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
                    Id = entity.ReferralId,
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

        public async Task<ApiResponse<SearchReply<ReferralModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "patientName" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<ReferralModel>>();


                IQueryable<ReferralModel> query = (from app in context.NHS_Referrals
                                                     join pat in context.NHS_Patients on app.PatientId equals pat.PatientId
                                                     select new ReferralModel
                                                     {
                                                         ReferralId = app.ReferralId,
                                                         PatientId = app.PatientId,
                                                         ConsultantId = app.ConsultantId,
                                                         DocumentName = app.DocumentName,
                                                         PatientName = pat.FirstName + " " + pat.LastName,
                                                         SpecialtyId = app.SpecialtyId,
                                                         ReferralDate = app.ReferralDate,
                                                         ConsultantName = app.ConsultantName,
                                                         Specialty = context.Specialties.FirstOrDefault(x => x.SpecialtyId == app.SpecialtyId).Name
                                                     }).AsQueryable();
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.ConsultantName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.DocumentName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.PatientName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Specialty.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                 );
                }
                switch (sortField)
                {
                    case "patientName":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientName) : query.OrderByDescending(s => s.PatientName);
                        break;
                    case "consultantName":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.ConsultantName) : query.OrderByDescending(s => s.ConsultantName);
                        break;

                    default:
                        query = query.OrderBy(s => s.ConsultantName);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<ReferralModel>()
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
                return new ApiResponse<SearchReply<ReferralModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<ReferralModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<ReferralModel>>> Get(long ReferralId)
        {
            try
            {
                if (ReferralId <= 0)
                {
                    return new ApiResponse<GetResponse<ReferralModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<ReferralModel> { Status = false, Entity = null, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<ReferralModel>>();

                var result = await context.NHS_Referrals.FirstOrDefaultAsync(x => x.ReferralId == ReferralId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<ReferralModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<ReferralModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<ReferralModel>()
                {
                    Status = true,
                    Entity =  new ReferralModel {
                        ReferralId = result.ReferralId,
                        PatientId = result.PatientId,
                        SpecialtyId = result.SpecialtyId,
                        ConsultantId = result.ConsultantId,
                        ConsultantName = result.ConsultantName,
                        DocumentExtension = result.DocumentExtension,
                        DocumentFile = result.DocumentFile,
                        DocumentName = result.DocumentName,
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
                return new ApiResponse<GetResponse<ReferralModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<ReferralModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long ReferralId)
        {
            try
            {

                if (ReferralId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "ReferralId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Referrals.Find(ReferralId);

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

                var details = $"Deleted Referral Information: ";
                await auditTrail.SaveAuditTrail(details, "Referral Information", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "ReferralId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.NHS_Referrals.FindAsync(item);
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

                var details = $"Deleted Multiple Referral Information: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Referral Information", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}