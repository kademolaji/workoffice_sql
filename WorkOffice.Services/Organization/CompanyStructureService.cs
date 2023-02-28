
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
    public class CompanyStructureService : ICompanyStructureService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public CompanyStructureService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(CompanyStructureModel model)
        {
            try
            {
                CompanyStructure parentInfo = new CompanyStructure();

                if (model.CompanyStructureId != Guid.Empty)
                {
                    return await Update(model);
                }
                if (model.StructureTypeId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Structure Type is required" }, IsSuccess = false };
                }
                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Name is required." }, IsSuccess = false };
                }
                var isExist = await context.CompanyStructures.AnyAsync(x => x.Name == model.Name);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.Name} already exist." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Address is required." }, IsSuccess = false };
                }
                if(model.ParentID != Guid.Empty)
                {
                    parentInfo = context.CompanyStructures.Find(model.ParentID);
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                CompanyStructure entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        model.Parent = parentInfo.Name;
                        model.Company = model.Company;

                        entity = model.ToModel<CompanyStructure>();
                        context.CompanyStructures.Add(entity);
                    
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new Company Structure: Name = {model.Name}, StructureTypeId = {model.CompanyStructureId}, CompanyHead = {model.CompanyHead} ";
                          await auditTrail.SaveAuditTrail(details, "Company Structure", "Create");
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
                    Id = entity.CompanyStructureId,
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

        public async Task<ApiResponse<CreateResponse>> Update(CompanyStructureModel model)
        {
            try
            {
                CompanyStructure parentInfo = new CompanyStructure();

                if (model.StructureTypeId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Structure Type is required" }, IsSuccess = false };
                }
                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Name is required." }, IsSuccess = false };
                }
                var isExist = await context.CompanyStructures.AnyAsync(x => x.Name == model.Name && x.CompanyStructureId != model.CompanyStructureId && x.ClientId == model.ClientId);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Another company structure exist with the given name {model.Name}." }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Address is required." }, IsSuccess = false };
                }
                if (model.ParentID != Guid.Empty)
                {
                    parentInfo = context.CompanyStructures.Find(model.ParentID);
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.CompanyStructures.FindAsync(model.CompanyStructureId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Name = model.Name;
                        entity.StructureTypeID = model.StructureTypeId;
                        entity.Country = model.Country;
                        entity.Parent = parentInfo.Name;
                        entity.Address = model.Address;
                        entity.CompanyHead = model.CompanyHead;
                        entity.ParentID = model.ParentID;
                        entity.Company = model.Name;
                        entity.ContactEmail = model.ContactEmail;
                        entity.ContactPhone = model.ContactPhone;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Company Structure: Name = {model.Name}, Address = {model.Address}, Head = {model.CompanyHead} ";
                            await auditTrail.SaveAuditTrail(details, "Company Structure", "Update");
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
                    Id = entity.CompanyStructureId,
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

        public async Task<ApiResponse<GetResponse<List<CompanyStructureModel>>>> GetList(Guid clientId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<CompanyStructureModel>>>();

                IQueryable<CompanyStructure> query = context.CompanyStructures.Where(x=>x.ClientId == clientId);
                int offset = (pageNumber - 1) * pageSize;
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();
                if (items.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<CompanyStructureModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<CompanyStructureModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<CompanyStructureModel>>()
                {
                    Status = true,
                    Entity = items.Select(x => x.ToModel<CompanyStructureModel>()).ToList(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<CompanyStructureModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<CompanyStructureModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<CompanyStructureModel>>> Get(Guid companyStructureId, Guid clientId)
        {
            try
            {
                if (companyStructureId == Guid.Empty)
                {
                    return new ApiResponse<GetResponse<CompanyStructureModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<CompanyStructureModel> { Status = false, Entity = null, Message = "CompanyStructureId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<CompanyStructureModel>>();

                var result = await context.CompanyStructures.FirstOrDefaultAsync(x=>x.CompanyStructureId == companyStructureId && x.ClientId == clientId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<CompanyStructureModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<CompanyStructureModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<CompanyStructureModel>()
                {
                    Status = true,
                    Entity = result.ToModel<CompanyStructureModel>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<CompanyStructureModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<CompanyStructureModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(Guid companyStructureId)
        {
            try
            {
                if (companyStructureId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "CompanyStructureId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.CompanyStructures.Find(companyStructureId);

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

                var details = $"Deleted Company Structure: Name = {result.Name}, Head = {result.CompanyHead}, Level = {result.Parent} ";
               await auditTrail.SaveAuditTrail(details, "Company Structure", "Delete");

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
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                foreach (var item in model.targetIds)
                {
                    var data = await context.CompanyStructures.FindAsync(item);
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

                var details = $"Deleted Multiple Company Structure: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Company Structure", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<byte[]>>> Export(Guid clientId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("StructureType");
                dt.Columns.Add("Country");
                dt.Columns.Add("Parent");
                dt.Columns.Add("Address");
                dt.Columns.Add("ContactEmail");
                dt.Columns.Add("ContactPhone");
                dt.Columns.Add("Head");
                dt.Columns.Add("ParentID");
                dt.Columns.Add("Company");

                var apiResponse = new ApiResponse<GetResponse<byte[]>>();

                var structures = await (from a in context.CompanyStructures
                                        where a.ClientId == clientId && a.IsDeleted == false
                                        select new CompanyStructureModel
                                        {
                                            CompanyStructureId = a.CompanyStructureId,
                                            Name = a.Name,
                                          //  StructureType = a.StructureType,
                                            Country = a.Country,
                                            Parent = a.Parent,
                                            Address = a.Address,
                                            CompanyHead = a.CompanyHead,
                                            ContactEmail = a.ContactEmail,
                                            ContactPhone = a.ContactPhone,
                                            ParentID = a.ParentID,
                                            Company = a.Company,
                                            ClientId = a.ClientId
                                        }).ToListAsync();

                if (structures.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in structures)
                {
                    var row = dt.NewRow();

                    row["Name"] = kk.Name;
                 //   row["StructureType"] = kk.StructureType;
                    row["Country"] = kk.Country;
                    row["Parent"] = kk.Parent;
                    row["Address"] = kk.Address;
                    row["ContactEmail"] = kk.ContactEmail;
                    row["ContactPhone"] = kk.ContactPhone;
                    row["Head"] = kk.CompanyHead;
                    row["ParentID"] = kk.ParentID;
                    row["Company"] = kk.Company;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (structures != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("CompanyStructure");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }
                var response = new GetResponse<byte[]>()
                {
                    Status = true,
                    Entity = fileBytes,
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Downloaded Company Structure: TotalCount {structures.Count} ";
                await auditTrail.SaveAuditTrail(details, "Company Structure", "Download");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<CompanyStructureModel> uploadedRecord = new List<CompanyStructureModel>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new CompanyStructureModel
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                         //   StructureType = workSheet.Cells[i, 2].Value.ToString(),
                            Country = workSheet.Cells[i, 3].Value.ToString(),
                            Parent = workSheet.Cells[i, 4].Value.ToString(),
                            Address = workSheet.Cells[i, 5].Value.ToString(),
                            ContactEmail = workSheet.Cells[i, 6].Value.ToString(),
                            ContactPhone = workSheet.Cells[i, 7].Value.ToString(),
                            CompanyHead = workSheet.Cells[i, 8].Value.ToString(),
                            ParentID = Guid.Parse(workSheet.Cells[i, 9].Value.ToString()),
                            Company = workSheet.Cells[i, 10].Value.ToString(),
                        }) ;
                    }
                }
                List<CompanyStructure> structures = new List<CompanyStructure>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var structure = new CompanyStructure
                        {
                            CompanyStructureId = item.CompanyStructureId,
                            Name = item.Name,
                            StructureTypeID = item.StructureTypeId,
                            Country = item.Country,
                            Parent = item.Parent,
                            Address = item.Address,
                            ContactEmail = item.ContactEmail,
                            ContactPhone = item.ContactPhone,
                            CompanyHead = item.CompanyHead,
                            ParentID = item.ParentID,
                            Company = item.Company,
                            IsDeleted = false,
                            ClientId = clientId,
                        };
                        structures.Add(structure);
                    }
                    context.CompanyStructures.AddRange(structures);
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

                var details = $"Uploaded Company Structure: TotalCount {structures.Count} ";
                await auditTrail .SaveAuditTrail(details, "Company Structure", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
