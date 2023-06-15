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
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetUserRoleList(long clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetCompanyList(long clientId);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetNHSActivity();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetAppType();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetConsultant();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetHospital();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPathwayStatus();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetRTT();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetSpecialty();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetWaitingType();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetWard();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPatientList(string search);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetDepartmentList();
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPatientPathWayList(string search);
        Task<ApiResponse<GetResponse<List<GeneralSettingsModel>>>> GetPathWayListByPatientId(long patientId, string search);

    }
}
