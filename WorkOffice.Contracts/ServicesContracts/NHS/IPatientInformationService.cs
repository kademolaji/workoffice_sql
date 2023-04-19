using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
    public interface IPatientInformationService
    {
        Task<ApiResponse<CreateResponse>> Create(PatientInformationModel model);
        Task<ApiResponse<SearchReply<PatientInformationModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<PatientInformationModel>>> Get(long PatientId);
        Task<ApiResponse<DeleteReply>> Delete(long PatientId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
