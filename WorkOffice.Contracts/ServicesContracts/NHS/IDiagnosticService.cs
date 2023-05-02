using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IDiagnosticService
    {
        Task<ApiResponse<CreateResponse>> Create(DiagnosticModel model);
        //Task<ApiResponse<CreateResponse>> Update(DiagnosticModel model);
        Task<ApiResponse<SearchReply<DiagnosticModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<DiagnosticModel>>> Get(long DiagnosticId);
        Task<ApiResponse<DeleteReply>> Delete(long DiagnosticId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
