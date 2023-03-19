using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IGeneralSettingsService
    {
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCountryList(Guid clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetStructureDefinitionList(Guid clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCompanyStructureList(Guid clientId);

    }
}
