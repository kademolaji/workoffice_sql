using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IAuthenticationService
    {
        Task<ApiResponse<GetResponse<UserAccountModel>>> Login(string userName, string password);
        Task<ApiResponse<CreateResponse>> Create(UserAccountModel model, string password);
        List<string> GetUserActivitiesByUser(Guid userAccountId);
    }
}
