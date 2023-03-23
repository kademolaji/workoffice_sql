using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IAppTypeService
    {
        Task<ApiResponse<CreateResponse>> CreateAppType(AppTypeViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateAppType(AppTypeViewModels model);
        //Task<ApiResponse<GetResponse<List<AppTypeViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<SearchReply<AppTypeViewModels>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<AppTypeViewModels>>> Get(Guid locationId);
        Task<ApiResponse<DeleteReply>> Delete(Guid locationId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId);
    }
}
