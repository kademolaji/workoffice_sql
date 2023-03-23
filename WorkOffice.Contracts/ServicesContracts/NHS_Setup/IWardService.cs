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
        Task<ApiResponse<CreateResponse>> UpdateWard(WardViewModels model);
        Task<ApiResponse<GetResponse<List<WardViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<WardViewModels>>> Get(long wardId);
        Task<ApiResponse<DeleteReply>> Delete(long wardId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long wardId);
    }
}
