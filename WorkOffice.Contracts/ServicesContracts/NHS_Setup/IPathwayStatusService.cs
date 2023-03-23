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
        Task<ApiResponse<CreateResponse>> UpdatePathwayStatus(PathwayStatusViewModels model);
        Task<ApiResponse<GetResponse<List<PathwayStatusViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<PathwayStatusViewModels>>> Get(long pathwayStatusId);
        Task<ApiResponse<DeleteReply>> Delete(long pathwayStatusId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long pathwayStatusId);
    }
}
