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
        Task<ApiResponse<GetResponse<StructureDefinitionModel>>> Get(long structureDefinitionId, long clientId);
        Task<ApiResponse<SearchReply<StructureDefinitionModel>>> GetList(SearchCall<SearchParameter> options, long clientId);
        Task<ApiResponse<DeleteReply>> Delete(long structureDefinitionId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export(long clientId);
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long clientId);
    }
}
