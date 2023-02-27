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
        Task<ApiResponse<GetResponse<List<CompanyStructureModel>>>> GetList(Guid clientId, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<CompanyStructureModel>>> Get(Guid companyStructureId, Guid clientId);
        Task<ApiResponse<DeleteReply>> Delete(Guid companyStructureId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export(Guid clientId);
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId);
    }
}
