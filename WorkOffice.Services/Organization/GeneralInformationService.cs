using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
   public class GeneralInformationService : IGeneralInformationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public GeneralInformationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(GeneralInformationModel model)
        {
            try
            {
                if (model.GeneralInformationId  != Guid.Empty)
                {
                    return await Update(model);
                }

                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Organisationname))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Name is required." }, IsSuccess = false };
                }
                var isExist = await context.GeneralInformations.AnyAsync(x => x.Organisationname == model.Organisationname);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.Organisationname} already exist." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                GeneralInformation entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = model.ToModel<GeneralInformation>();
                        context.GeneralInformations.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new General Information: Definition = {model.ClientId}, Description = {model.Organisationname}, Level = {model.State} ";
                            await auditTrail.SaveAuditTrail(details, "General Information", "Create");
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
                    Id = entity.GeneralInformationId,
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

        public async Task<ApiResponse<CreateResponse>> Update(GeneralInformationModel model)
        {
            try
            {

                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Organisationname))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.GeneralInformations.FindAsync(model.GeneralInformationId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Organisationname = model.Organisationname;
                        entity.Taxid = model.Taxid;
                        entity.Regno = model.Regno;
                        entity.Phone = model.Phone;
                        entity.Email = model.Email;
                        entity.Fax = model.Fax;
                        entity.Address1 = model.Address1;
                        entity.Address2 = model.Address2;
                        entity.City = model.City;
                        entity.State = model.State;
                        entity.Country = model.Country;
                        entity.Note = model.Note;
                        entity.Zipcode = model.Zipcode;
                        entity.Currency = model.Currency;
                        entity.ImgLogo = model.ImgLogo;
                        entity.Imgtype = model.Imgtype;
                        entity.Ismulticompany = model.Ismulticompany;
                        entity.Subsidiary_level = model.Subsidiary_level;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated GeneralInformation: Definition = {model.ClientId}, Description = {model.Organisationname}, Level = {model.Regno} ";
                            await auditTrail.SaveAuditTrail(details, "GeneralInformation", "Update");
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
                    Id = entity.GeneralInformationId,
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

        public async Task<ApiResponse<GetResponse<GeneralInformationModel>>> Get(Guid generalInformationId, Guid clientId)
        {
            try
            {
                if (generalInformationId == Guid.Empty)
                {
                    return new ApiResponse<GetResponse<GeneralInformationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<GeneralInformationModel> { Status = false, Entity = null, Message = "GeneralInformationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<GeneralInformationModel>>();

                var result = await context.GeneralInformations.FirstOrDefaultAsync(x=>x.GeneralInformationId == generalInformationId && x.ClientId == clientId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<GeneralInformationModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<GeneralInformationModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<GeneralInformationModel>()
                {
                    Status = true,
                    Entity = result.ToModel<GeneralInformationModel>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<GeneralInformationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<GeneralInformationModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(Guid generalInformationId)
        {
            try
            {
                if (generalInformationId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.Locations.FindAsync(generalInformationId);

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

                var details = $"Deleted Location: Definition = {result.Name}, Description = {result.Address}, Level = {result.Country} ";
                await auditTrail.SaveAuditTrail(details, "Location", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
