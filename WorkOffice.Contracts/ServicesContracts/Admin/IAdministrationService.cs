using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.Admin;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Contracts.ServicesContracts.Admin
{
    public interface IAdministrationService
    {
        Task<ApiResponse<CreateResponse>> CreateUserRole(CreateUserRoleRequest model);
    }
}
