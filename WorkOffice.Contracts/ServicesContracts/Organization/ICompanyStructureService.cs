using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface ICompanyStructureService
    {
        Task<ApiResponse<CreateResponse>> Create(CompanyStructureModel model);
        Task<ApiResponse<CreateResponse>> Update(CompanyStructureModel model);
        Task<ApiResponse<SearchReply<CompanyStructureModel>>> GetList(SearchCall<SearchParameter> options, long clientId);
        Task<ApiResponse<GetResponse<CompanyStructureModel>>> Get(long companyStructureId, long clientId);
        Task<ApiResponse<DeleteReply>> Delete(long companyStructureId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export(long clientId);
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long clientId);
    }
}
