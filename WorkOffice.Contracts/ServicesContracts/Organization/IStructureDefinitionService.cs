using H2RHRMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace H2RHRMS.Core.Interfaces.Services
{
    public interface IStructureDefinitionService
    {
        Task<ApiResponse<CreateResponse>> Create(StructureDefinitionModel model);
        Task<ApiResponse<GetResponse<StructureDefinitionModel>>> Get(long structureDefinitionId, int clientId);
        Task<ApiResponse<GetResponse<List<StructureDefinitionModel>>>> GetList(int clientId, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<DeleteReply>> Delete(long structureDefinitionId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export(int clientId);
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, int clientId);
    }
}
