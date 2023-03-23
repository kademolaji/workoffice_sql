using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IRTTService
    {
        Task<ApiResponse<CreateResponse>> CreateRTT(RTTViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateRTT(RTTViewModels model);
        Task<ApiResponse<GetResponse<List<RTTViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<RTTViewModels>>> Get(long rttId);
        Task<ApiResponse<DeleteReply>> Delete(long rttId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long rttId);
    }
}
