using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.Models.Shared;

namespace WorkOffice.Contracts.ServicesContracts.Shared
{
    public interface IAuditTrailService
    {
        Task SaveAuditTrail(string details, string page, string actionType);
        Task<ApiResponse<SearchReply<SearchAuditTrailModel>>> SearchAuditTrail(SearchCall<string> options);
        Task SaveNotification(List<SaveNotificationModel> notificationModels);
    }
}
