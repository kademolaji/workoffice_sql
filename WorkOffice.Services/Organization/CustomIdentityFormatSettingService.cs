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

        public async Task<ApiResponse<GetResponse<List<CustomIdentityFormatSettingModel>>>> GetList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<CustomIdentityFormatSettingModel>>>();

                IQueryable<CustomIdentityFormatSetting> query = context.CustomIdentityFormatSettings;
                int offset = (pageNumber - 1) * pageSize;
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();
                if (items.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<CustomIdentityFormatSettingModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<CustomIdentityFormatSettingModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<CustomIdentityFormatSettingModel>>()
                {
                    Status = true,
                    Entity = items.Select(x => x.ToModel<CustomIdentityFormatSettingModel>()).ToList(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<CustomIdentityFormatSettingModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<CustomIdentityFormatSettingModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
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
    }
}
