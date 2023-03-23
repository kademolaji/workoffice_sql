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
//using WorkOffice.Contracts.Mappings;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class AppTypeService: IAppTypeService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public AppTypeService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }
        public async Task<ApiResponse<CreateResponse>> CreateAppType(AppTypeViewModels model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Code))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "AppType Code is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "AppType Name is required." }, IsSuccess = false };
                }
               

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                AppType entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {

                        entity = model.ToModel<AppType>();
                        context.AppTypes.Add(entity);

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                          
                            context.SaveChanges();
                         
                            var details = $"Created new AppType:Code = {model.Code}, Name = {model.Name}";
                            await auditTrail.SaveAuditTrail(details, "AppType", "Create");
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
                    Id = entity.AppTypeId,
                    Status = true,
                    Message = "AppType created successfully"
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

        public async Task<ApiResponse<CreateResponse>> UpdateAppType(AppTypeViewModels model)
        {
            try
            {

                if (model.AppTypeId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "AppTypeId is required" }, IsSuccess = false };
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
                var entity = await context.AppTypes.FindAsync(model.AppTypeId);
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

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated AppType: Code = {model.Code}, Name = {model.Name} ";
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
                    Id = entity.AppTypeId,
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

        public async Task<ApiResponse<SearchReply<AppTypeViewModels>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "code" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<AppTypeViewModels>>();


                IQueryable<AppType> query = context.AppTypes;
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


                var response = new SearchReply<AppTypeViewModels>()
                {
                    TotalCount = count,
                    Result = items.Select(x => x.ToModel<AppTypeViewModels>()).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<AppTypeViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<AppTypeViewModels>() { TotalCount = count }, IsSuccess = false };
            }
        }

        //public async Task<ApiResponse<GetResponse<List<AppTypeViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10)
        //{
        //    try
        //    {
        //        var apiResponse = new ApiResponse<GetResponse<List<AppTypeViewModels>>>();

        //        IQueryable<AppType> query = context.AppTypes;
        //        int offset = (pageNumber - 1) * pageSize;
        //        var items = await query.Skip(offset).Take(pageSize).ToListAsync();
        //        if (items.Count <= 0)
        //        {
        //            return new ApiResponse<GetResponse<List<AppTypeViewModels>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<AppTypeViewModels>>() { Status = false, Message = "No record found" }, IsSuccess = false };
        //        }


        //        var response = new GetResponse<List<AppTypeViewModels>>()
        //        {
        //            Status = true,
        //            Entity = items.Select(x =>x.ToModel<AppTypeViewModels>()).ToList(),
        //            Message = ""
        //        };

        //        apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //        apiResponse.IsSuccess = true;
        //        apiResponse.ResponseType = response;

        //        return apiResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse<GetResponse<List<AppTypeViewModels>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<AppTypeViewModels>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
        //    }
        //}

        public async Task<ApiResponse<GetResponse<AppTypeViewModels>>> Get(Guid appTypeId)
        {
            try
            {
                if (appTypeId == Guid.Empty)
                {
                    return new ApiResponse<GetResponse<AppTypeViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AppTypeViewModels> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<AppTypeViewModels>>();

                var result = await context.AppTypes.FindAsync(appTypeId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<AppTypeViewModels>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<AppTypeViewModels>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<AppTypeViewModels>()
                {
                    Status = true,
                    Entity = result.ToModel<AppTypeViewModels>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AppTypeViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AppTypeViewModels>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(Guid appTypeId)
        {
            try
            {
                if (appTypeId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "AppTypeId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.AppTypes.FindAsync(appTypeId);

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

                var details = $"Deleted AppType: Code = {result.Code}, Name = {result.Name} ";
                await auditTrail.SaveAuditTrail(details, "AppType", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "AppTypeId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.AppTypes.FindAsync(item);
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

                var details = $"Deleted Multiple AppType: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "AppType", "Delete");

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

                var appType = await (from a in context.AppTypes
                                        where a.IsDeleted == false
                                        select new AppTypeViewModels
                                        {
                                            Code = a.Code,
                                            Name = a.Name,
                                        }).ToListAsync();

                if (appType.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in appType)
                {
                    var row = dt.NewRow();
                    row["Code"] = kk.Code;
                    row["Name"] = kk.Name;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (appType != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("AppType");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }

                var details = $"Downloaded AppType: TotalCount {appType.Count} ";
                await auditTrail.SaveAuditTrail(details, "AppType", "Download");

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

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid appTypeId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<AppTypeViewModels> uploadedRecord = new List<AppTypeViewModels>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new AppTypeViewModels
                        {
                            Code = workSheet.Cells[i, 1].Value.ToString(),
                            Name = workSheet.Cells[i, 2].Value.ToString(),
                        });
                    }
                }
                List<AppType> appTypes = new List<AppType>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var appType = new AppType
                        {
                            Code = item.Code,
                            Name = item.Name,
                            IsDeleted = false,
                        };
                        appTypes.Add(appType);
                    }
                    context.AppTypes.AddRange(appTypes);
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

                var details = $"Uploaded Apptype: TotalCount {appTypes.Count} ";
                await auditTrail.SaveAuditTrail(details, "AppType", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
