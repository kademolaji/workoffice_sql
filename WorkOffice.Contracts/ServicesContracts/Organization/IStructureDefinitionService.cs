using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IStructureDefinitionService
    {
        Task<ApiResponse<CreateResponse>> Create(StructureDefinitionModel model);
        Task<ApiResponse<GetResponse<StructureDefinitionModel>>> Get(Guid structureDefinitionId, Guid clientId);
        Task<ApiResponse<SearchReply<StructureDefinitionModel>>> GetList(SearchCall<SearchParameter> options, Guid clientId);
        Task<ApiResponse<DeleteReply>> Delete(string structureDefinitionId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export(Guid clientId);
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId);
    }
}
