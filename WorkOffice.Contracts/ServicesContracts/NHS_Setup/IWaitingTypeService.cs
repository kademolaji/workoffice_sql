using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IWaitingTypeService
    {
        Task<ApiResponse<CreateResponse>> CreateWaitingType(WaitingTypeViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateWaitingType(WaitingTypeViewModels model);
        Task<ApiResponse<GetResponse<List<WaitingTypeViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<WaitingTypeViewModels>>> Get(long waitingTypeId);
        Task<ApiResponse<DeleteReply>> Delete(long waitingTypeId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long waitingTypeId);
    }
}
