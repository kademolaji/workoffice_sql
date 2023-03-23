using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IGeneralSettingsService
    {
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCountryList(long clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetStructureDefinitionList(long clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCompanyStructureList(long clientId);

    }
}
