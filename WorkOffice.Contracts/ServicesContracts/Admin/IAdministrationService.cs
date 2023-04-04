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
        Task<ApiResponse<GetResponse<List<UserRoleAndActivityModel>>>> GetAllUserRoleDefinitions(long clientId);
        Task<ApiResponse<GetResponse<UserRoleAndActivityModel>>> GetUserRoleDefinition(long userRoleId, long clientId);
        Task<ApiResponse<DeleteReply>> DeleteUserRoleDefinition(long userRoleDefinitionId);
        Task<ApiResponse<DeleteReply>> DeleteMultipleUserRoleDefinition(MultipleDeleteModel model);




        Task<ApiResponse<GetResponse<List<UserActivityParentModel>>>> GetActivities(long clientId);
        Task<ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>> GetUserRoleAndActivities(long clientId, long userRoleId);
        Task<ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>> GetActivitiesByRoleId(long clientId, long userRoleId);
    }
}
