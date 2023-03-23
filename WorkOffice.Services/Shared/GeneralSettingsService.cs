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
    }
}
