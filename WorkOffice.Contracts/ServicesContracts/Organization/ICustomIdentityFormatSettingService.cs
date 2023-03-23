
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
        Task<ApiResponse<GetResponse<List<CustomIdentityFormatSettingModel>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<GetResponse<CustomIdentityFormatSettingModel>>> Get(long employeeIdFormatId);
        Task<ApiResponse<DeleteReply>> Delete(long employeeIdFormatId);
    }
}
