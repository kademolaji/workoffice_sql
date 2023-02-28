using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IAdministrationService
    {
        Task<ApiResponse<CreateResponse>> AddUpdateUserRoleAndActivity(UserRoleAndActivityModel model);
        Task<ApiResponse<GetResponse<List<UserRoleDefinitionModel>>>> GetAllUserRoleDefinitions(Guid clientId);
        Task<ApiResponse<DeleteReply>> DeleteUserRoleDefinition(Guid userRoleDefinitionId);
        Task<ApiResponse<DeleteReply>> DeleteMultipleUserRoleDefinition(MultipleDeleteModel model);




        Task<ApiResponse<GetResponse<List<UserActivityParentModel>>>> GetActivities(Guid clientId);
        Task<ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>> GetUserRoleAndActivities(Guid clientId, Guid userRoleId);
        Task<ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>> GetActivitiesByRoleId(Guid clientId, Guid userRoleId);
    }
}
