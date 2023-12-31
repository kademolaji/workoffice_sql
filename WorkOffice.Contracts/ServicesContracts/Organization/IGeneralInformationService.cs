﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;

namespace WorkOffice.Contracts.ServicesContracts
{
   public interface IGeneralInformationService
    {
        Task<ApiResponse<CreateResponse>> Create(GeneralInformationModel model);
        Task<ApiResponse<CreateResponse>> Update(GeneralInformationModel model);
        Task<ApiResponse<SearchReply<GeneralInformationModel>>> GetList(SearchCall<SearchParameter> options, long clientId);
        Task<ApiResponse<GetResponse<GeneralInformationModel>>> Get(long generalInformationId, long clientId);
        Task<ApiResponse<DeleteReply>> Delete(long generalInformationId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
    }
}
