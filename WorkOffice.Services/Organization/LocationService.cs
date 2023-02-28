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
   public class LocationService : IlocationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public LocationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(LocationModel model)
        {
            try
            {
                if (model.LocationId != Guid.Empty)
                {
                    return await Update(model);
                }
               
                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }
                var isExist = await context.Locations.AnyAsync(x => x.Name == model.Name);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.Name} already exist." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                Location entity = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = model.ToModel<Location>();
                        context.Locations.Add(entity);

                      
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new Location: Definition = {model.ClientId.ToString()}, Description = {model.Name}, Level = {model.State} ";
                           await auditTrail.SaveAuditTrail(details, "Location", "Create");
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
                    Id = entity.LocationId,
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

        public async Task<ApiResponse<CreateResponse>> Update(LocationModel model)
        {
            try
            {
              
                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Definition is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.Locations.FindAsync(model.LocationId);
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.Name = model.Name;
                        entity.Country = model.Country;
                        entity.State = model.State;
                        entity.City = model.City;
                        entity.Address = model.Address;
                        entity.ZipCode = model.ZipCode;
                        entity.Phone = model.Phone;
                        entity.Fax = model.Fax;
                        entity.Note = model.Note;
                                               
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Location: Definition = {model.ClientId}, Description = {model.Name}, Level = {model.Address} ";
                          await  auditTrail.SaveAuditTrail(details, "Location", "Update");
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
                    Id = entity.LocationId,
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

        public async Task<ApiResponse<GetResponse<List<LocationModel>>>> GetList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<LocationModel>>>();

                IQueryable<Location> query = context.Locations;
                int offset = (pageNumber - 1) * pageSize;
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();
                if (items.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<LocationModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<LocationModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<LocationModel>>()
                {
                    Status = true,
                    Entity = items.Select(x => x.ToModel<LocationModel>()).ToList(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<LocationModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<LocationModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<LocationModel>>> Get(Guid locationId)
        {
            try
            {
                if (locationId == Guid.Empty)
                {
                    return new ApiResponse<GetResponse<LocationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<LocationModel> { Status = false, Entity = null, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<LocationModel>>();

                var result = await context.Locations.FindAsync(locationId);

                if (result == null)
                {
                    return new ApiResponse<GetResponse<LocationModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<LocationModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<LocationModel>()
                {
                    Status = true,
                    Entity = result.ToModel<LocationModel>(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<LocationModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<LocationModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(Guid locationId)
        {
            try
            {
                if (locationId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "LocationId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = await context.Locations.FindAsync(locationId);

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

                var details = $"Deleted Location: Definition = {result.Name}, Description = {result.Address}, Level = {result.Country} ";
                await auditTrail .SaveAuditTrail(details, "Location", "Delete");

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
                    var data = await context.Locations.FindAsync(item);
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

                var details = $"Deleted Multiple Location: with Ids {model.targetIds.ToArray()} ";
                await auditTrail .SaveAuditTrail(details, "Location", "Delete");

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
                dt.Columns.Add("Name");
                dt.Columns.Add("Country");
                dt.Columns.Add("State");
                dt.Columns.Add("City");
                dt.Columns.Add("Address");
                dt.Columns.Add("ZipCode");
                dt.Columns.Add("Phone");
                dt.Columns.Add("Fax");
                dt.Columns.Add("Note");
                var apiResponse = new ApiResponse<GetResponse<byte[]>>();

                var structures = await (from a in context.Locations
                                        where a.IsDeleted == false
                                        select new LocationModel
                                        {
                                            LocationId = a.LocationId,
                                            Name = a.Name,
                                            Country = a.Country,
                                            State = a.State,
                                            City = a.City,
                                            Address = a.Address,
                                            ZipCode = a.ZipCode,
                                            Phone = a.Phone,
                                            Fax = a.Fax,
                                            Note = a.Note,
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
                    row["Country"] = kk.Country;
                    row["State"] = kk.State;
                    row["City"] = kk.City;
                    row["Address"] = kk.Address;
                    row["ZipCode"] = kk.ZipCode;
                    row["Phone"] = kk.Phone;
                    row["Fax"] = kk.Fax;
                    row["Note"] = kk.Note;

                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (structures != null)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Location");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }

                var details = $"Downloaded Location: TotalCount {structures.Count} ";
               await auditTrail.SaveAuditTrail(details, "Location", "Download");

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

        public async Task<ApiResponse<CreateResponse>> Upload(byte[] record, Guid clientId)
        {
            try
            {
                var apiResponse = new ApiResponse<CreateResponse>();
                if (record == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Upload a valid record." }, IsSuccess = false };
                }

                List<LocationModel> uploadedRecord = new List<LocationModel>();

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new LocationModel
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            Country = workSheet.Cells[i, 2].Value.ToString(),
                            State = workSheet.Cells[i, 3].Value.ToString(),
                            City = workSheet.Cells[i, 4].Value.ToString(),
                            Address = workSheet.Cells[i, 5].Value.ToString(),
                            ZipCode = workSheet.Cells[i, 6].Value.ToString(),
                            Phone = workSheet.Cells[i, 7].Value.ToString(),
                            Fax = workSheet.Cells[i, 8].Value.ToString(),
                            Note = workSheet.Cells[i, 9].Value.ToString(),
                        });
                    }
                }
                List<Location> structures = new List<Location>();
                if (uploadedRecord.Count > 0)
                {

                    foreach (var item in uploadedRecord)
                    {
                        var structure = new Location
                        {
                            Name = item.Name,
                            Country = item.Country,
                            State = item.State,
                            City = item.City,
                            Address = item.Address,
                            ZipCode = item.ZipCode,
                            Phone = item.Phone,
                            Fax = item.Fax,
                            Note = item.Note,
                            IsDeleted = false,
                            ClientId = clientId,
                        };
                        structures.Add(structure);
                    }
                    context.Locations.AddRange(structures);
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

                var details = $"Uploaded Location: TotalCount {structures.Count} ";
               await auditTrail.SaveAuditTrail(details, "Location", "Upload");

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }
    }
}
