using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        private readonly DataContext context;
        public GeneralSettingsService(DataContext appContext)
        {
            context = appContext;
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCompanyStructureList(long clientId)
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.CompanyStructures
                                    where a.ClientId == clientId && a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.CompanyStructureId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCountryList(long clientId)
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Countries
                                    where a.ClientId == clientId && a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.CountryId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetStructureDefinitionList(long clientId)
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.StructureDefinitions
                                    where a.ClientId == clientId && a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Definition,
                                        Value = a.StructureDefinitionId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetUserRoleList(long clientId)
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.UserRoleDefinitions
                                    where a.ClientId == clientId && a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.RoleName,
                                        Value = a.UserRoleDefinitionId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCompanyList(long clientId)
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.GeneralInformations
                                    where a.ClientId == clientId && a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Organisationname,
                                        Value = a.GeneralInformationId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

  

public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetNHSActivity()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.NHSActivities
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.NHSActivityId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetAppType()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.AppTypes
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.AppTypeId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetConsultant()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Consultants
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.ConsultantId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetHospital()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Hospitals
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.HospitalId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPathwayStatus()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.PathwayStatuses
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.PathwayStatusId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetRTT()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.RTTs
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.RTTId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetSpecialty()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Specialties
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.SpecialtyId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetWaitingType()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.WaitingTypes
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.WaitingTypeId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetWard()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Wards
                                    where a.IsDeleted == false
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.WardId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPatientList()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.NHS_Patients
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.DistrictNumber,
                                        Value = a.PatientId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
        public async Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetDepartmentList()
        {
            try
            {

                var apiResponse = new ApiResponse<GetResponse<List<GeneralSettingsModel>>>();

                var result = await (from a in context.Departments
                                    select new GeneralSettingsModel
                                    {
                                        Label = a.Name,
                                        Value = a.DepartmentId.ToString()
                                    }).ToListAsync();

                var response = new GetResponse<List<GeneralSettingsModel>>()
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
                return new ApiResponse<GetResponse<List<GeneralSettingsModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<GeneralSettingsModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
     
    }
}
