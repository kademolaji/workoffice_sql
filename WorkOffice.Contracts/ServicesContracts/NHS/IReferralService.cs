using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IReferralService
    {
        Task<ApiResponse<CreateResponse>> Create(ReferralModel model);
        Task<ApiResponse<SearchReply<ReferralModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<ReferralModel>>> Get(long ReferralId);
        Task<ApiResponse<DeleteReply>> Delete(long ReferralId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
