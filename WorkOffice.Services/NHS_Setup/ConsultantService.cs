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
    public class ConsultantService: IConsultantService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public ConsultantService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }
        public async Task<ApiResponse<CreateResponse>> CreateConsultant(ConsultantViewModels model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Code))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "Consultant Code is required." }, IsSuccess = false };
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse { Status = false, Message = "Consultant Name is required." }, IsSuccess = false };
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                Consultant entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {

                        entity = model.ToModel<Consultant>();
                        context.Consultants.Add(entity);

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {

                            context.SaveChanges();

                            var details = $"Created new Consultant:Code = {model.Code}, Name = {model.Name}";
                            await auditTrail.SaveAuditTrail(details, "Consultant", "Create");
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
                    Id = entity.ConsultantId,
                    Status = true,
                    Message = "Consultant created successfully"
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

        public async Task<ApiResponse<CreateResponse>> UpdateConsultant(ConsultantViewModels model)
        {
            try
            {

                if (model.ConsultantId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "ConsultantId is required" }, IsSuccess = false };
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
                var entity = await context.Consultants.FindAsync(model.ConsultantId);
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
                            var details = $"Updated Consultant: Code = {model.Code}, Name = {model.Name} ";
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
                    Id = entity.ConsultantId,
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

        public async Task<ApiResponse<GetResponse<List<ConsultantViewModels>>>> GetList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<ConsultantViewModels>>>();

                IQueryable<Consultant> query = context.Consultants;
                int offset = (pageNumber - 1) * pageSize;
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();
                if (items.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<ConsultantViewModels>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<ConsultantViewModels>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<ConsultantViewModels>>()
                {
                    Status = true,
                    Entity = items.Select(x => x.ToModel<ConsultantViewModels>()).ToList(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<ConsultantViewModels>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<ConsultantViewModels>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<ConsultantViewModels>>> Get(Guid consultantId)
        {
            try
            {
                if (consultantId == Guid.Empty)
                {
                    return new ApiResponse<GetResponse<ConsultantViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<ConsultantViewModels> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<ConsultantViewModels>>();

                var result = await context.Consultants.FindAsync(consultantId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<ConsultantViewModels>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<ConsultantViewModels>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<ConsultantViewModels>()
                {
                    Status = true,
                    Entity = result.ToModel<ConsultantViewModels>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<ConsultantViewModels>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<ConsultantViewModels>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(Guid consultantId)
        {
            try
            {
                if (consultantId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "ConsultantId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.Consultants.FindAsync(consultantId);

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

                var details = $"Deleted Consultant: Code = {result.Code}, Name = {result.Name} ";
                await auditTrail.SaveAuditTrail(details, "Consultant", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "ConsultantId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.Consultants.FindAsync(item);
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

                var details = $"Deleted Multiple Consultant: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Consultant", "Delete");

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

                var consult = await (from a in context.Consultants
                                     where a.IsDeleted == false
                                     select new ConsultantViewModels
                                     {
                                         Code = a.Code,
                                         Name = a.Name,
                                     }).ToListAsync();

                if (consult.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in consult)
                {
                    var row = dt.NewRow();
                    row["Code"] = kk.Code;
                    row["Name"] = kk.Name;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (consult != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Consultant");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }

                var details = $"Downloaded Consultant: TotalCount {consult.Count} ";
                await auditTrail.SaveAuditTrail(details, "Consultant", "Download");

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

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid consultantId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<ConsultantViewModels> uploadedRecord = new List<ConsultantViewModels>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new ConsultantViewModels
                        {
                            Code = workSheet.Cells[i, 1].Value.ToString(),
                            Name = workSheet.Cells[i, 2].Value.ToString(),
                        });
                    }
                }
                List<Consultant> consultants = new List<Consultant>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var consultant = new Consultant
                        {
                            Code = item.Code,
                            Name = item.Name,
                            IsDeleted = false,
                        };
                        consultants.Add(consultant);
                    }
                    context.Consultants.AddRange(consultants);
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

                var details = $"Uploaded Apptype: TotalCount {consultants.Count} ";
                await auditTrail.SaveAuditTrail(details, "Consultant", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


    }
}
