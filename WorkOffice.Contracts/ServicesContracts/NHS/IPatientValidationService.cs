using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IPatientValidationService
    {
        Task<ApiResponse<CreateResponse>> Create(PatientValidationModel model);
        Task<ApiResponse<CreateResponse>> Update(PatientValidationModel model);
        Task<ApiResponse<SearchReply<PatientValidationModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<PatientValidationModel>>> Get(long PatientValidationId);
        Task<ApiResponse<GetResponse<PatientValidationModel>>> GetSinglePatientValidation(long PatientId);
        Task<ApiResponse<DeleteReply>> Delete(long PatientValidationId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<DeleteReply>> PatientDetailsValidation(long PatientValidationDetailsId);
        Task<ApiResponse<GetResponse<List<PatientValidationModel>>>> GetPathwayByPatientId(long patientId);

    }
}
