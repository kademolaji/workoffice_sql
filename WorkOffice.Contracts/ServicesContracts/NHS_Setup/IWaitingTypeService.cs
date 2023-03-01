using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.NHS_Setup;


namespace WorkOffice.Contracts.ServicesContracts.NHS_Setup
{
    public interface IWaitingTypeService
    {
        Task<ApiResponse<CreateResponse>> CreateWaitingType(WaitingTypeViewModels model);
    }
}
