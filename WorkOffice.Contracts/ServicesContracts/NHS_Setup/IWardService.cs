using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IWardService
    {
        Task<ApiResponse<CreateResponse>> CreateWard(WardViewModels model);
    }
}
