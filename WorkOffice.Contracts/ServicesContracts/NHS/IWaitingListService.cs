using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IWaitingListService
    {
        Task<ApiResponse<CreateResponse>> Create(WaitingListModel model);
        Task<ApiResponse<SearchReply<WaitingListModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<WaitingListModel>>> Get(long waitinglistId);
        Task<ApiResponse<DeleteReply>> Delete(long waitinglistId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
