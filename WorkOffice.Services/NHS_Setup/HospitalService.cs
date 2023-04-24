using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Mappings;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class HospitalService: IHospitalService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public HospitalService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }
        public async Task<ApiResponse<CreateResponse>> CreateHospital(HospitalViewModels model)
        {
            try
            {

                if (model.HospitalId > 0)
                {
                    return await UpdateHospital(model);
                }
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
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {

                        entity = model.ToModel<Hospital>();
                        context.Hospitals.Add(entity);

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {

                            context.SaveChanges();

                            var details = $"Created new Hospital:Code = {model.Code}, Name = {model.Name}";
                            await auditTrail.SaveAuditTrail(details, "Hospital", "Create");
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

        public async Task<ApiResponse<CreateResponse>> UpdateHospital(HospitalViewModels model)
        {
            try
            {

                if (model.HospitalId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "HospitalId is required" }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(model.Code))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Code is required." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Name is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.Hospitals.FindAsync(model.HospitalId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Code = model.Code;
                        entity.Name = model.Name;
                        entity.CreatedOn = DateTime.UtcNow;
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Hospital: Code = {model.Code}, Name = {model.Name} ";
                            await auditTrail.SaveAuditTrail(details, "App Type", "Update");
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
                    Id = entity.HospitalId,
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
        public async Task<ApiResponse<SearchReply<HospitalViewModels>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "code" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<HospitalViewModels>>();


                IQueryable<Hospital> query = context.Hospitals;
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.Code.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Name.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "code":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Code) : query.OrderByDescending(s => s.Code);
                        break;
                    case "name":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Name) : query.OrderByDescending(s => s.Name);
                        break;
                    default:
                        query = query.OrderBy(s => s.Code);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<HospitalViewModels>()
                {
                    TotalCount = count,
                    Result = items.Select(x => x.ToModel<HospitalViewModels>()).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<HospitalViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<HospitalViewModels>() { TotalCount = count }, IsSuccess = false };
            }
        }

        //public async Task<ApiResponse<GetResponse<List<HospitalViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10)
        //{
        //    try
        //    {
        //        var apiResponse = new ApiResponse<GetResponse<List<HospitalViewModels>>>();

        //        IQueryable<Hospital> query = context.Hospitals;
        //        int offset = (pageNumber - 1) * pageSize;
        //        var items = await query.Skip(offset).Take(pageSize).ToListAsync();
        //        if (items.Count <= 0)
        //        {
        //            return new ApiResponse<GetResponse<List<HospitalViewModels>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<HospitalViewModels>>() { Status = false, Message = "No record found" }, IsSuccess = false };
        //        }


        //        var response = new GetResponse<List<HospitalViewModels>>()
        //        {
        //            Status = true,
        //            Entity = items.Select(x => x.ToModel<HospitalViewModels>()).ToList(),
        //            Message = ""
        //        };

        //        apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //        apiResponse.IsSuccess = true;
        //        apiResponse.ResponseType = response;

        //        return apiResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<GetResponse<List<HospitalViewModels>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<HospitalViewModels>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
        //    }
        //}

        public async Task<ApiResponse<GetResponse<HospitalViewModels>>> Get(long hospitalId)
        {
            try
            {
                if (hospitalId <= 0)
                {
                    return new ApiResponse<GetResponse<HospitalViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<HospitalViewModels> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<HospitalViewModels>>();

                var result = await context.Hospitals.FindAsync(hospitalId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<HospitalViewModels>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<HospitalViewModels>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<HospitalViewModels>()
                {
                    Status = true,
                    Entity = result.ToModel<HospitalViewModels>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<HospitalViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<HospitalViewModels>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long hospitalId)
        {
            try
            {
                if (hospitalId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "HospitalId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.Hospitals.FindAsync(hospitalId);

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                result.IsDeleted = true;


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Hospital: Code = {result.Code}, Name = {result.Name} ";
                await auditTrail.SaveAuditTrail(details, "Hospital", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "HospitalId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.Hospitals.FindAsync(item);
                    if (data != null)
                    {
                        data.IsDeleted = true;
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

                var details = $"Deleted Multiple Hospital: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Hospital", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<byte[]>>> Export()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Code");
                dt.Columns.Add("Name");
                var apiResponse = new ApiResponse<GetResponse<byte[]>>();

                var hospital = await (from a in context.Hospitals
                                     where a.IsDeleted == false
                                     select new HospitalViewModels
                                     {
                                         Code = a.Code,
                                         Name = a.Name,
                                     }).ToListAsync();

                if (hospital.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in hospital)
                {
                    var row = dt.NewRow();
                    row["Code"] = kk.Code;
                    row["Name"] = kk.Name;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (hospital != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Hospital");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }

                var details = $"Downloaded Hospital: TotalCount {hospital.Count} ";
                await auditTrail.SaveAuditTrail(details, "Hospital", "Download");

                var response = new GetResponse<byte[]>()
                {
                    Status = true,
                    Entity = fileBytes,
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;


                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, long hospitalId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<HospitalViewModels> uploadedRecord = new List<HospitalViewModels>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new HospitalViewModels
                        {
                            Code = workSheet.Cells[i, 1].Value.ToString(),
                            Name = workSheet.Cells[i, 2].Value.ToString(),
                        });
                    }
                }
                List<Hospital> hospitals = new List<Hospital>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var hospital = new Hospital
                        {
                            Code = item.Code,
                            Name = item.Name,
                            IsDeleted = false,
                        };
                        hospitals.Add(hospital);
                    }
                    context.Hospitals.AddRange(hospitals);
                }


                var result = await context.SaveChangesAsync() > 0;

                var response = new CreateResponse
                {
                    Status = result,
                    Id = "",
                    Message = "Record Uploaded Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Uploaded Apptype: TotalCount {hospitals.Count} ";
                await auditTrail.SaveAuditTrail(details, "Hospital", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
