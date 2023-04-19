using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IConsultantService
    {
        Task<ApiResponse<CreateResponse>> CreateConsultant(ConsultantViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateConsultant(ConsultantViewModels model);
        //Task<ApiResponse<GetResponse<List<ConsultantViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<SearchReply<ConsultantViewModels>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<ConsultantViewModels>>> Get(long consultantId);
        Task<ApiResponse<DeleteReply>> Delete(long consultantId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long clientId);
    }
}
