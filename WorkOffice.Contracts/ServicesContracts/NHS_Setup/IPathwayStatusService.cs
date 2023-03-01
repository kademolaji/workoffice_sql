using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IPathwayStatusService
    {
        Task<ApiResponse<CreateResponse>> CreatePathwayStatus(PathwayStatusViewModels model);
    }
}
