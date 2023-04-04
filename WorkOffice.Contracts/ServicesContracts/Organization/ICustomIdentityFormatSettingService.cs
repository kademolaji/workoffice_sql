
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
  public  interface ICustomIdentityFormatSettingService
    {
        Task<ApiResponse<CreateResponse>> Create(CustomIdentityFormatSettingModel model);
        Task<ApiResponse<CreateResponse>> Update(CustomIdentityFormatSettingModel model);
        Task<ApiResponse<SearchReply<CustomIdentityFormatSettingModel>>> GetList(SearchCall<SearchParameter> options, long clientId);
        Task<ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>> Get(long employeeIdFormatId);
        Task<ApiResponse<DeleteReply>> Delete(long employeeIdFormatId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
