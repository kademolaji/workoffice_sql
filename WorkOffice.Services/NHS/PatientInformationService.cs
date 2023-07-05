using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Common;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class PatientInformationService : IPatientInformationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public PatientInformationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(PatientInformationModel model)
        {
            try
            {

                if (model.PatientId > 0)
                {
                    return await Update(model);
                }
                if (string.IsNullOrEmpty(model.FirstName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "First Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.LastName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Last Name is required." }, IsSuccess = false };
                }
                var activity = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Information).Replace("_", " ");
                var districtNumber = GenerateSerialNumber(activity);
                if (string.IsNullOrEmpty(districtNumber))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "District Number cannot be generated, contact admin for setup." }, IsSuccess = false };
                }

                var activityNHS = Enum.GetName(typeof(UserActivitiesEnum), UserActivitiesEnum.Patient_Document).Replace("_", " ");
                var nhsNumber = GenerateSerialNumber(activityNHS);
                if (string.IsNullOrEmpty(nhsNumber))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "NHS Number cannot be generated, contact admin for setup." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Patient entity = null;

                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Patient
                        {
                            DistrictNumber = districtNumber,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            DOB = model.DOB,
                            Age = model.Age,
                            Address = model.Address,
                            PhoneNo = model.PhoneNo,
                            Email = model.Email,
                            Sex = model.Sex,
                            PostalCode = model.PostalCode,
                            NHSNumber = nhsNumber,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Patients.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Patient Information: FirstName = {model.FirstName}, LastName = {model.LastName}, DistrictNumber = {model.DistrictNumber} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Information", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.PatientId,
                    Message = "Record created successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> Update(PatientInformationModel model)
        {
            try
            {


                if (string.IsNullOrEmpty(model.FirstName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "First Name is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.LastName))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Last Name is required." }, IsSuccess = false };
                }

                var existingNHSNumber = context.NHS_Patients.Any(x => x.NHSNumber == model.NHSNumber && x.PatientId != model.PatientId);
                if (existingNHSNumber)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another Patient already exists with the given NHS Number" }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Patients.FindAsync(model.PatientId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.FirstName = model.FirstName;
                        entity.LastName = model.LastName;
                        entity.MiddleName = model.MiddleName;
                        entity.DOB = model.DOB;
                        entity.Age = model.Age;
                        entity.Address = model.Address;
                        entity.PhoneNo = model.PhoneNo;
                        entity.Email = model.Email;
                        entity.Sex = model.Sex;
                        entity.PostalCode = model.PostalCode;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Patient Information: FirstName = {model.FirstName}, LastName = {model.LastName}, DistrictNumber = {model.DistrictNumber} ";
                            await auditTrail.SaveAuditTrail(details, "Patient Information", "Update");
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
                    }
                }

                var response = new CreateResponse
                {
                    Status = result,
                    Id = entity.PatientId,
                    Message = "Record updated successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<SearchReply<PatientInformationModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "firstName" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<PatientInformationModel>>();


                IQueryable<NHS_Patient> query = context.NHS_Patients;
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.FirstName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.LastName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.MiddleName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Address.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.DistrictNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.NHSNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "firsName":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.FirstName) : query.OrderByDescending(s => s.FirstName);
                        break;
                    case "email":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Email) : query.OrderByDescending(s => s.Email);
                        break;

                    default:
                        query = query.OrderBy(s => s.FirstName);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<PatientInformationModel>()
                {
                    TotalCount = count,
                    Result = items.Select(x => new PatientInformationModel
                    {
                        PatientId = x.PatientId,
                        DistrictNumber = x.DistrictNumber,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MiddleName = x.MiddleName,
                        DOB = x.DOB,
                        Age = x.Age,
                        Address = x.Address,
                        PhoneNo = x.PhoneNo,
                        Email = x.Email,
                        Sex = x.Sex,
                        PostalCode = x.PostalCode,
                        NHSNumber = x.NHSNumber,
                        FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}"
                    }).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<PatientInformationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<PatientInformationModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<PatientInformationModel>>> Get(long PatientId)
        {
            try
            {
                if (PatientId <= 0)
                {
                    return new ApiResponse<GetResponse<PatientInformationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientInformationModel> { Status = false, Entity = null, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<PatientInformationModel>>();

                var result = await context.NHS_Patients.FirstOrDefaultAsync(x => x.PatientId == PatientId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<PatientInformationModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<PatientInformationModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<PatientInformationModel>()
                {
                    Status = true,
                    Entity = new PatientInformationModel
                    {
                        PatientId = result.PatientId,
                        DistrictNumber = result.DistrictNumber,
                        FirstName = result.FirstName,
                        LastName = result.LastName,
                        MiddleName = result.MiddleName,
                        DOB = result.DOB,
                        Age = result.Age,
                        Address = result.Address,
                        PhoneNo = result.PhoneNo,
                        Email = result.Email,
                        Sex = result.Sex,
                        PostalCode = result.PostalCode,
                        NHSNumber = result.NHSNumber,
                        FullName = $"{result.FirstName} {result.MiddleName} {result.LastName}"
                    },
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<PatientInformationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<PatientInformationModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long PatientId)
        {
            try
            {

                if (PatientId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Patients.Find(PatientId);

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                result.Deleted = true;


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Patient Information: DistrictNumber = {result.DistrictNumber}, FirstName = {result.FirstName}, LastName = {result.LastName} ";
                await auditTrail.SaveAuditTrail(details, "Patient Information", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> MultipleDelete(MultipleDeleteModel model)
        {
            try
            {
                if (model.targetIds.Count <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.NHS_Patients.FindAsync(item);
                    if (data != null)
                    {
                        data.Deleted = true;
                    }
                };

                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Records Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Multiple Patient Information: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Patient Information", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }



        public string GenerateSerialNumber(string activity)
        {
            int randomNumber = 0;

            var customSettings = context.CustomIdentityFormatSettings.FirstOrDefault(x => x.Activity.ToLower().Trim() == activity.ToLower().Trim());
            if (customSettings != null)
            {
                long lastDigit = customSettings.LastDigit + 1;

                string generatedNumber = randomNumber.ToString().PadLeft(customSettings.Digits - 1, '0');
                string serialNumber = customSettings.Prefix + customSettings.Separator + generatedNumber + lastDigit + customSettings.Separator + customSettings.Suffix;
                customSettings.LastDigit = lastDigit;
                context.SaveChanges();
                return serialNumber;
            }
            return null;
        }
    }

}