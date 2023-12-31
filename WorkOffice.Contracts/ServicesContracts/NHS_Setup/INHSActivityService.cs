﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;


namespace WorkOffice.Contracts.ServicesContracts
{
    public interface INHSActivityService
    {
        Task<ApiResponse<CreateResponse>> CreateActivity(NHSActivityViewModels model);
        Task<ApiResponse<CreateResponse>> UpdateActivity(NHSActivityViewModels model);
        //Task<ApiResponse<GetResponse<List<NHSActivityViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<SearchReply<NHSActivityViewModels>>> GetList(SearchCall<SearchParameter> options);
        Task<ApiResponse<GetResponse<NHSActivityViewModels>>> Get(long nhsActivityId);
        Task<ApiResponse<DeleteReply>> Delete(long nhsActivityId);
        Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model);
        Task<ApiResponse<GetResponse<byte[]>>> Export();
        Task<ApiResponse<CreateResponse>> Upload(byte[] record, long nhsActivityId);
    }
}
