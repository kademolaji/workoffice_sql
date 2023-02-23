﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.Shared;

namespace WorkOffice.Contracts.ServicesContracts.NHS_Setup
{
    public interface IPathwayStatusService
    {
        Task<ApiResponse<CreateResponse>> CreatePathwayStatus(PathwayStatusViewModels model);
    }
}
