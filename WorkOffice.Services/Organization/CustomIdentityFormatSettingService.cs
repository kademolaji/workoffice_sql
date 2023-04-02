using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Mappings;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
   public class CustomIdentityFormatSettingService : ICustomIdentityFormatSettingService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public CustomIdentityFormatSettingService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(CustomIdentityFormatSettingModel model)
        {
            try
            {
                if (model.CustomIdentityFormatSettingId > 0)
                {
                    return await Update(model);
                }

                if (model.ClientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (model.Digits < 0 )
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }
                var isExist = await context.CustomIdentityFormatSettings.AnyAsync(x => x.Company == model.Company);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.Digits} already exist." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                CustomIdentityFormatSetting entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = model.ToModel<CustomIdentityFormatSetting>();
                        context.CustomIdentityFormatSettings.Add(entity);

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new CustomIdentityFormatSetting";
                           await  auditTrail.SaveAuditTrail(details, "CustomIdentityFormatSetting", "Create");
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
                    Id = entity.CustomIdentityFormatSettingId,
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

        public async Task<ApiResponse<CreateResponse>> Update(CustomIdentityFormatSettingModel model)
        {
            try
            {

                if (model.ClientId <=0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (model.Digits <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.CustomIdentityFormatSettings.FindAsync(model.CustomIdentityFormatSettingId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Prefix = model.Prefix;
                        entity.Suffix = model.Suffix;
                        entity.Digits = model.Digits;
                        entity.Company = model.Company;

                       
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated CustomIdentityFormatSetting ";
                            await auditTrail.SaveAuditTrail(details, "CustomIdentityFormatSetting", "Update");
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
                    Id = entity.CustomIdentityFormatSettingId,
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

        public async Task<ApiResponse<SearchReply<CustomIdentityFormatSettingModel>>> GetList(SearchCall<SearchParameter> options, long clientId)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "prefix" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<CustomIdentityFormatSettingModel>>();

                IQueryable<CustomIdentityFormatSetting> query = context.CustomIdentityFormatSettings.Where(x => x.ClientId == clientId);
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.Suffix.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Prefix.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "prefix":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Prefix) : query.OrderByDescending(s => s.Prefix);
                        break;
                    case "suffix":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Suffix) : query.OrderByDescending(s => s.Suffix);
                        break;
                    case "company":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Company) : query.OrderByDescending(s => s.Company);
                        break;
                    default:
                        query = query.OrderBy(s => s.Suffix);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<CustomIdentityFormatSettingModel>()
                {
                    TotalCount = count,
                    Result = items.Select(x => x.ToModel<CustomIdentityFormatSettingModel>()).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<CustomIdentityFormatSettingModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<CustomIdentityFormatSettingModel>() { TotalCount = 0, }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>> Get(long employeeIdFormatId)
        {
            try
            {
                if (employeeIdFormatId <=0)
                {
                    return new ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<CustomIdentityFormatSettingModel> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>();

                var result = await context.CustomIdentityFormatSettings.FindAsync(employeeIdFormatId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<CustomIdentityFormatSettingModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<CustomIdentityFormatSettingModel>()
                {
                    Status = true,
                    Entity = result.ToModel<CustomIdentityFormatSettingModel>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<CustomIdentityFormatSettingModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long employeeIdFormatId)
        {
            try
            {
                if (employeeIdFormatId <=0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.CustomIdentityFormatSettings.FindAsync(employeeIdFormatId);

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                result.IsDeleted = true;


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted CustomIdentityFormatSettings ";
                await auditTrail.SaveAuditTrail(details, "CustomIdentityFormatSettings", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.CustomIdentityFormatSettings.FindAsync(item);
                    if (data != null)
                    {
                        data.IsDeleted = true;
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

                var details = $"Deleted Multiple Custom Identity Format Settings: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Custom Identity Format Settings", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
