using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface ISpecialtyService
    {
        Task<ApiResponse<CreateResponse>> CreateSpecialty(SpecialtyViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateSpecialty(SpecialtyViewModels model);
        //Task<ApiResponse<GetResponse<List<SpecialtyViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<SearchReply<SpecialtyViewModels>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<SpecialtyViewModels>>> Get(long specialtyId);
        Task<ApiResponse<DeleteReply>> Delete(long specialtyId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long specialtyId);
    }
}
