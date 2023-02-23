using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.NHS_Setup;
using WorkOffice.Contracts.ServicesContracts.Shared;
using WorkOffice.Domain.Entities.NHS_Setup;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services.NHS_Setup
{
    public class HospitalService: IHospitalService
    {
        private readonly DataContext _context;
        private readonly IAuditTrailService _auditTrail;

        public HospitalService(DataContext context, IAuditTrailService auditTrail)
        {
            _context = context;
            _auditTrail = auditTrail;
        }
        public async Task<ApiResponse<CreateResponse>> CreateHospital(HospitalViewModels model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Code))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "Hospital Code is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "Hospital Name is required." }, IsSuccess = false };
                }
               

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                Hospital entity = null;
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new Hospital
                        {
                            Code = model.Code,
                            Name = model.Name,
                        };
                        _context.Hospitals.Add(entity);

                        result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                          
                            _context.SaveChanges();
                         
                            var details = $"Created new Hospital:Code = {model.Code}, Name = {model.Name}";
                            await _auditTrail.SaveAuditTrail(details, "Hospital", "Create");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

               
                var response = new CreateResponse()
                {
                    Id = entity.HospitalId,
                    Status = true,
                    Message = "Hospital created successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
