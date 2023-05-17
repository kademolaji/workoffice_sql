using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IAppointmentsServices
    {
        Task<ApiResponse<CreateResponse>> Create(CreateAppointmentModel model);
        Task<ApiResponse<SearchReply<AppointmentResponseModel>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<AppointmentResponseModel>>> Get(int appointmentId);
        Task<ApiResponse<DeleteReply>> Delete(int appointmentId);
        Task<ApiResponse<DeleteReply>> Cancel(int appointmentId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
