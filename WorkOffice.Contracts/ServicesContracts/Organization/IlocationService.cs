using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
  public  interface IlocationService
    {
        Task<ApiResponse<CreateResponse>> Create(LocationModel model);
        Task<ApiResponse<CreateResponse>> Update(LocationModel model);
        Task<ApiResponse<GetResponse<List<LocationModel>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<LocationModel>>> Get(Guid locationId);
        Task<ApiResponse<DeleteReply>> Delete(Guid locationId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId);
    }
}
