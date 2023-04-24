using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IPatientDocumentService
    {
        Task<ApiResponse<CreateResponse>> Create(PatientDocumentModel model);
        Task<ApiResponse<SearchReply<PatientDocumentModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<DeleteReply>> Delete(long patientDocumentId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<PatientDocumentModel>>> Get(long patientDocumentId);
    }
}
