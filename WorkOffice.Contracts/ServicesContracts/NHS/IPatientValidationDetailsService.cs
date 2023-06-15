using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IPatientValidationDetailsService
    {
        Task<ApiResponse<CreateResponse>> Create(PatientValidationDetailsModel model);
        Task<ApiResponse<CreateResponse>> Merge(MergePathwayModel model);
        Task<ApiResponse<SearchReply<PatientValidationDetailsModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<PatientValidationDetailsModel>>> Get(long patientValidationDetailsId);
        Task<ApiResponse<DeleteReply>> DeletePatientDetailsValidation(long patientValidationDetailsId);
    }
}
