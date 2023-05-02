using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IDiagnosticResultService
    {
        Task<ApiResponse<CreateResponse>> Create(DiagnosticResultModel model);
        Task<ApiResponse<SearchReply<DiagnosticResultModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<DeleteReply>> Delete(long DiagnosticResultId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<DiagnosticResultModel>>> Get(long DiagnosticResultId);
    }
}
