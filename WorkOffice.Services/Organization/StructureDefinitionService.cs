﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using OfficeOpenXml;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Helpers;
using WorkOffice.Contracts.Models;
using WorkOffice.Domain.Entities;
using WorkOffice.Contracts.Mappings;

namespace WorkOffice.Services
{
    public class StructureDefinitionService : IStructureDefinitionService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public StructureDefinitionService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(StructureDefinitionModel model)
        {
            try
            {
           
                if (model.StructureDefinitionId  > 0)
                {
                    return await Update(model);
                }
                if (string.IsNullOrEmpty(model.Definition))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }
                if (model.Level <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Level cannot be less than or equal to Zero." }, IsSuccess = false };
                }
                if (model.ClientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                var isLevelExist = await context.StructureDefinitions.AnyAsync(x => x.Level == model.Level && x.ClientId == model.ClientId);

                if (isLevelExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Level {model.Level} already exist." }, IsSuccess = false };
                }

                var isExist = await context.StructureDefinitions.AnyAsync(x => x.Definition == model.Definition && x.ClientId == model.ClientId);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.Definition} already exist." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                StructureDefinition entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = model.ToModel<StructureDefinition>();
                        context.StructureDefinitions.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new Structure Definition: Definition = {model.Definition}, Description = {model.Description}, Level = {model.Level} ";
                            await auditTrail.SaveAuditTrail(details, "Structure Definition", "Create");
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
                    Id = entity.StructureDefinitionId,
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

        public async Task<ApiResponse<CreateResponse>> Update(StructureDefinitionModel model)
        {
            try
            {
               

                if (string.IsNullOrEmpty(model.Definition))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }
                if (model.Level <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Level cannot be less than or equal to Zero." }, IsSuccess = false };
                }
                if (model.ClientId <=0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }

                var existingDefinition = context.StructureDefinitions.Any(x => x.Definition == model.Definition && x.StructureDefinitionId != model.StructureDefinitionId && x.ClientId == model.ClientId);
                if (existingDefinition)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Another Structure Definition already exists with the given definition" }, IsSuccess = false };
                }
                var isLevelExist = await context.StructureDefinitions.AnyAsync(x => x.Level == model.Level && x.StructureDefinitionId != model.StructureDefinitionId && x.ClientId == model.ClientId);

                if (string.IsNullOrEmpty(model.Definition))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Another Structure Definition already exists with the given level" }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.StructureDefinitions.FindAsync(model.StructureDefinitionId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Level = model.Level;
                        entity.Description = model.Description;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Structure Definition: Definition = {model.Definition}, Description = {model.Description}, Level = {model.Level} ";
                            await auditTrail.SaveAuditTrail(details, "Structure Definition", "Update");
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
                    Id = entity.StructureDefinitionId,
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

        public async Task<ApiResponse<SearchReply<StructureDefinitionModel>>> GetList(SearchCall<SearchParameter> options, long clientId)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "definition" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<StructureDefinitionModel>>();


                IQueryable<StructureDefinition> query = context.StructureDefinitions.Where(x => x.ClientId == clientId);
                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    query = query.Where(x => x.Definition.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Description.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                }
                switch (sortField)
                {
                    case "definition":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Definition) : query.OrderByDescending(s => s.Definition);
                        break;
                    case "description":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Description) : query.OrderByDescending(s => s.Description);
                        break;
                    case "level":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Level) : query.OrderByDescending(s => s.Level);
                        break;
                    default:
                        query = query.OrderBy(s => s.Definition);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<StructureDefinitionModel>()
                {
                    TotalCount = count,
                    Result = items.Select(x => x.ToModel<StructureDefinitionModel>()).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<StructureDefinitionModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<StructureDefinitionModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<StructureDefinitionModel>>> Get(long structureDefinitionId, long clientId)
        {
            try
            {
                if (structureDefinitionId <=0)
                {
                    return new ApiResponse<GetResponse<StructureDefinitionModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<StructureDefinitionModel> { Status = false, Entity = null, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<StructureDefinitionModel>>();

                var result = await context.StructureDefinitions.FirstOrDefaultAsync(x => x.StructureDefinitionId == structureDefinitionId && x.ClientId == clientId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<StructureDefinitionModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<StructureDefinitionModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<StructureDefinitionModel>()
                {
                    Status = true,
                    Entity = result.ToModel<StructureDefinitionModel>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<StructureDefinitionModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<StructureDefinitionModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(long structureDefinitionId)
        {
            try
            {
              
                if (structureDefinitionId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "StructureDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.StructureDefinitions.Find(structureDefinitionId);

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

                var details = $"Deleted Structure Definition: Definition = {result.Definition}, Description = {result.Description}, Level = {result.Level} ";
                await auditTrail.SaveAuditTrail(details, "Structure Definition", "Delete");

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
                    var data = await context.StructureDefinitions.FindAsync(item);
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

                var details = $"Deleted Multiple Structure Definition: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Structure Definition", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<byte[]>>> Export(long clientId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("Description");
                dt.Columns.Add("Level");
                var apiResponse = new ApiResponse<GetResponse<byte[]>>();

                var structures = await (from a in context.StructureDefinitions
                                        where a.ClientId == clientId && a.IsDeleted == false
                                        select new StructureDefinitionModel
                                        {
                                            StructureDefinitionId = a.StructureDefinitionId,
                                            Level = a.Level,
                                            Definition = a.Definition,
                                            Description = a.Description
                                        }).ToListAsync();

                if (structures.Count == 0)
                {
                    return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = "No record found." }, IsSuccess = false };
                }

                foreach (var kk in structures)
                {
                    var row = dt.NewRow();
                    row["Name"] = kk.Definition;
                    row["Description"] = kk.Description;
                    row["Level"] = kk.Level;
                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (structures != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("StructureDifinition");
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

                var details = $"Downloaded Structure Definition: TotalCount {structures.Count} ";
                await auditTrail.SaveAuditTrail(details, "Structure Definition", "Download");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<byte[]>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<byte[]> { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, long clientId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<StructureDefinitionModel> uploadedRecord = new List<StructureDefinitionModel>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new StructureDefinitionModel
                        {
                            Definition = workSheet.Cells[i, 1].Value.ToString(),
                            Description = workSheet.Cells[i, 2].Value.ToString(),
                            Level = int.Parse(workSheet.Cells[i, 3].Value.ToString()),
                        });
                    }
                }
                List<StructureDefinition> structures = new List<StructureDefinition>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        if (string.IsNullOrEmpty(item.Definition))
                        {
                            return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                        }
                        if (item.Level <= 0)
                        {
                            return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Level cannot be less than or equal to Zero." }, IsSuccess = false };
                        }
                        if (clientId <= 0)
                        {
                            return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                        }
                        var isLevelExist = await context.StructureDefinitions.AnyAsync(x => x.Level == item.Level && x.ClientId == clientId);

                        if (isLevelExist)
                        {
                            return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Level {item.Level} already exist." }, IsSuccess = false };
                        }
                        var isExist = await context.StructureDefinitions.AnyAsync(x => x.Definition == item.Definition && x.ClientId == clientId);
                        if (isExist)
                        {
                            return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{item.Definition} already exist." }, IsSuccess = false };
                        }

                        var structure = new StructureDefinition
                        {
                            Definition = item.Definition,
                            Description = item.Description,
                            Level = item.Level,
                            IsDeleted = false,
                            CreatedOn = DateTime.UtcNow,
                            ClientId = clientId,
                        };
                        structures.Add(structure);
                    }
                    context.StructureDefinitions.AddRange(structures);
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

                var details = $"Uploaded Structure Definition: TotalCount {structures.Count} ";
                await auditTrail.SaveAuditTrail(details, "Structure Definition", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
