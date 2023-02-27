
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IGeneralInformationService
    {
        Task<ApiResponse<CreateResponse>> Create(GeneralInformationModel model);
        Task<ApiResponse<CreateResponse>> Update(GeneralInformationModel model);
        Task<ApiResponse<GetResponse<GeneralInformationModel>>> Get(Guid generalInformationId, Guid clientId);
        Task<ApiResponse<DeleteReply>> Delete(Guid locationId);
    }
}
