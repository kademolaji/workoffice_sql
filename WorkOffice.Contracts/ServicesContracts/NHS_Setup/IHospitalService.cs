using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IHospitalService
    {
        Task<ApiResponse<CreateResponse>> CreateHospital(HospitalViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateHospital(HospitalViewModels model);
        //Task<ApiResponse<GetResponse<List<HospitalViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<SearchReply<HospitalViewModels>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<HospitalViewModels>>> Get(long hospitalId);
        Task<ApiResponse<DeleteReply>> Delete(long hospitalId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long hospitalId);
    }
}
