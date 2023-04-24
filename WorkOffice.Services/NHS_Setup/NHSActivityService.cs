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
    public class NHSActivityService: INHSActivityService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public NHSActivityService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }
        public async Task<ApiResponse<CreateResponse>> CreateActivity(NHSActivityViewModels model)
        {
            try
            {
                if (model.NHSActivityId > 0)
                {
                    return await UpdateActivity(model);
                }
                if (string.IsNullOrEmpty(model.Code))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "NHSActivity Code is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "NHSActivity Name is required." }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHSActivity entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {

                        entity = model.ToModel<NHSActivity>();
                        context.NHSActivities.Add(entity);

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {

                            context.SaveChanges();

                            var details = $"Created new NHSActivity:Code = {model.Code}, Name = {model.Name}";
                            await auditTrail.SaveAuditTrail(details, "NHSActivity", "Create");
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
                    Id = entity.NHSActivityId,
                    Status = true,
                    Message = "NHSActivity created successfully"
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

        public async Task<ApiResponse<CreateResponse>> UpdateActivity(NHSActivityViewModels model)
        {
            try
            {

                if (model.NHSActivityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "NHSActivityId is required" }, IsSuccess = false };
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
                var entity = await context.NHSActivities.FindAsync(model.NHSActivityId);
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
                            var details = $"Updated NHSActivity: Code = {model.Code}, Name = {model.Name} ";
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
                    Id = entity.NHSActivityId,
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

        public async Task<ApiResponse<SearchReply<NHSActivityViewModels>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "code" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<NHSActivityViewModels>>();


                IQueryable<NHSActivity> query = context.NHSActivities;
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


                var response = new SearchReply<NHSActivityViewModels>()
                {
                    TotalCount = count,
                    Result = items.Select(x => x.ToModel<NHSActivityViewModels>()).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<NHSActivityViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<NHSActivityViewModels>() { TotalCount = count }, IsSuccess = false };
            }
        }

        //public async Task<ApiResponse<GetResponse<List<NHSActivityViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10)
        //{
        //    try
        //    {
        //        var apiResponse = new ApiResponse<GetResponse<List<NHSActivityViewModels>>>();

        //        IQueryable<NHSActivity> query = context.NHSActivities;
        //        int offset = (pageNumber - 1) * pageSize;
        //        var items = await query.Skip(offset).Take(pageSize).ToListAsync();
        //        if (items.Count <= 0)
        //        {
        //            return new ApiResponse<GetResponse<List<NHSActivityViewModels>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<NHSActivityViewModels>>() { Status = false, Message = "No record found" }, IsSuccess = false };
        //        }


        //        var response = new GetResponse<List<NHSActivityViewModels>>()
        //        {
        //            Status = true,
        //            Entity = items.Select(x => x.ToModel<NHSActivityViewModels>()).ToList(),
        //            Message = ""
        //        };

        //        apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //        apiResponse.IsSuccess = true;
        //        apiResponse.ResponseType = response;

        //        return apiResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<GetResponse<List<NHSActivityViewModels>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<NHSActivityViewModels>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
        //    }
        //}

        public async Task<ApiResponse<GetResponse<NHSActivityViewModels>>> Get(long nhsActivityId)
        {
            try
            {
                if (nhsActivityId <= 0)
                {
                    return new ApiResponse<GetResponse<NHSActivityViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<NHSActivityViewModels> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<NHSActivityViewModels>>();

                var result = await context.NHSActivities.FindAsync(nhsActivityId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<NHSActivityViewModels>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<NHSActivityViewModels>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<NHSActivityViewModels>()
                {
                    Status = true,
                    Entity = result.ToModel<NHSActivityViewModels>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<NHSActivityViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<NHSActivityViewModels>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long nhsActivityId)
        {
            try
            {
                if (nhsActivityId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "NHSActivityId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.NHSActivities.FindAsync(nhsActivityId);

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

                var details = $"Deleted NHSActivity: Code = {result.Code}, Name = {result.Name} ";
                await auditTrail.SaveAuditTrail(details, "NHSActivity", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "NHSActivityId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.NHSActivities.FindAsync(item);
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

                var details = $"Deleted Multiple NHSActivity: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "NHSActivity", "Delete");

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

                var nhsActivity = await (from a in context.NHSActivities
                                         where a.IsDeleted == false
                                     select new NHSActivityViewModels
                                     {
                                         Code = a.Code,
                                         Name = a.Name,
                                     }).ToListAsync();

                if (nhsActivity.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in nhsActivity)
                {
                    var row = dt.NewRow();
                    row["Code"] = kk.Code;
                    row["Name"] = kk.Name;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (nhsActivity != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("NHSActivity");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }

                var details = $"Downloaded NHSActivity: TotalCount {nhsActivity.Count} ";
                await auditTrail.SaveAuditTrail(details, "NHSActivity", "Download");

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

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, long nhsActivityId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<NHSActivityViewModels> uploadedRecord = new List<NHSActivityViewModels>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new NHSActivityViewModels
                        {
                            Code = workSheet.Cells[i, 1].Value.ToString(),
                            Name = workSheet.Cells[i, 2].Value.ToString(),
                        });
                    }
                }
                List<NHSActivity> nhsActivitys = new List<NHSActivity>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var nhsActivity = new NHSActivity
                        {
                            Code = item.Code,
                            Name = item.Name,
                            IsDeleted = false,
                        };
                        nhsActivitys.Add(nhsActivity);
                    }
                    context.NHSActivities.AddRange(nhsActivitys);
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

                var details = $"Uploaded Apptype: TotalCount {nhsActivitys.Count} ";
                await auditTrail.SaveAuditTrail(details, "NHSActivity", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
