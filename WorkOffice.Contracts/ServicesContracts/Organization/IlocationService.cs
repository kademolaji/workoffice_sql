using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{ 
  public  interface IlocationService
    {
        Task<ApiResponse<CreateResponse>> Create(LocationModel model);
        Task<ApiResponse<CreateResponse>> Update(LocationModel model);
        Task<ApiResponse<SearchReply<LocationModel>>> GetList(SearchCall<SearchParameter> options, long clientId);
        Task<ApiResponse<GetResponse<LocationModel>>> Get(long locationId);
        Task<ApiResponse<DeleteReply>> Delete(long locationId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long clientId);
    }
}
