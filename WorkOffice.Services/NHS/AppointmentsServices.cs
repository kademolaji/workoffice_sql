using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;

namespace WorkOffice.Services
{
    public class AppointmentsServices : IAppointmentsServices
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public AppointmentsServices(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        public async Task<ApiResponse<CreateResponse>> Create(CreateAppointmentModel model)
        {
            try
            {

                if (model.AppointmentId > 0)
                {
                    return await Update(model);
                }
                if (model.PatientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Patient is required." }, IsSuccess = false };
                }
                if (model.AppTypeId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Appointment Type is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.StatusId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Status is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.ConsultantId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Consultant is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.WardId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Ward is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && (string.IsNullOrEmpty(model.BookDate) || string.IsNullOrEmpty(model.AppDate)))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Booking date and appointment date is required." }, IsSuccess = false };
                }
                if (model.StatusId == 1)
                {
                    model.AppointmentStatus = "ATTEND";
                }
                else if (model.StatusId == 2)
                {
                    model.AppointmentStatus = "CANCELLED BY PATIENT";
                }
                else if (model.StatusId == 3)
                {
                    model.AppointmentStatus = "CANCELLED BY HOSPITAL";
                }
                else if (model.StatusId == 5)
                {
                    model.AppointmentStatus = "FUTURE";
                }
                else
                {
                    model.AppointmentStatus = "DO NOT ATTEND";
                }


                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                NHS_Appointment entity = null;
                DateTime? appDate = null;
                if (!string.IsNullOrEmpty(model.AppDate))
                {
                    appDate = Convert.ToDateTime(model.AppDate);
                }
                DateTime? bookDate = null;
                if (!string.IsNullOrEmpty(model.BookDate))
                {
                    bookDate = Convert.ToDateTime(model.BookDate);
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity = new NHS_Appointment
                        {
                            AppTypeId = model.AppTypeId,
                            StatusId = model.StatusId,
                            SpecialtyId = model.SpecialityId,
                            BookDate = bookDate,
                            AppDate = appDate,
                            AppTime = model.AppTime,
                            ConsultantId = model.ConsultantId,
                            HospitalId = model.HospitalId,
                            WardId = model.WardId,
                            DepartmentId = model.DepartmentId,
                            PatientId = model.PatientId,
                            Comments = model.Comments,
                            AppointmentStatus = model.AppointmentStatus,
                            CancellationReason = model.CancellationReason,
                            PatientValidationId = model.PatientValidationId,
                            Active = true,
                            Deleted = false,
                            CreatedBy = model.CurrentUserName,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.NHS_Appointments.Add(entity);
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created New Appointment: Comments = {model.Comments}, AppointmentId = {entity.AppointmentId} ";
                            await auditTrail.SaveAuditTrail(details, "Appointment", "Update");
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
                    Id = entity.AppointmentId,
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

        public async Task<ApiResponse<CreateResponse>> Update(CreateAppointmentModel model)
        {
            try
            {


                if (model.PatientId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Patient is required." }, IsSuccess = false };
                }
                if (model.AppTypeId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Appointment Type is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.StatusId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Status is required." }, IsSuccess = false };
                }
                if (model.SpecialityId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Speciality is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.ConsultantId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Consultant is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && model.WardId <= 0)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Ward is required." }, IsSuccess = false };
                }
                if (model.AppTypeId != 3 && (string.IsNullOrEmpty(model.BookDate) || string.IsNullOrEmpty(model.AppDate)))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Booking date and appointment date is required." }, IsSuccess = false };
                }
                if (model.StatusId == 1)
                {
                    model.AppointmentStatus = "ATTEND";
                }
                else if (model.StatusId == 2)
                {
                    model.AppointmentStatus = "CANCELLED BY PATIENT";
                }
                else if (model.StatusId == 3)
                {
                    model.AppointmentStatus = "CANCELLED BY HOSPITAL";
                }
                else if (model.StatusId == 5)
                {
                    model.AppointmentStatus = "FUTURE";
                }
                else
                {
                    model.AppointmentStatus = "DO NOT ATTEND";
                }



                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                var entity = await context.NHS_Appointments.FindAsync(model.AppointmentId);
                DateTime? appDate = null;
                if (!string.IsNullOrEmpty(model.AppDate))
                {
                    appDate = Convert.ToDateTime(model.AppDate);
                }
                DateTime? bookDate = null;
                if (!string.IsNullOrEmpty(model.BookDate))
                {
                    bookDate = Convert.ToDateTime(model.BookDate);
                }
                if (entity == null)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Record does not exist." }, IsSuccess = false };
                }
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        entity.AppTypeId = model.AppTypeId;
                        entity.StatusId = model.StatusId;
                        entity.SpecialtyId = model.SpecialityId;
                        entity.BookDate = bookDate;
                        entity.AppDate = appDate;
                        entity.AppTime = model.AppTime;
                        entity.ConsultantId = model.ConsultantId;
                        entity.HospitalId = model.HospitalId;
                        entity.WardId = model.WardId;
                        entity.DepartmentId = model.DepartmentId;
                        entity.PatientId = model.PatientId;
                        entity.PatientValidationId = model.PatientValidationId;
                        entity.Comments = model.Comments;
                        entity.AppointmentStatus = model.AppointmentStatus;
                        entity.CancellationReason = model.CancellationReason;
                        entity.UpdatedBy = model.CurrentUserName;
                        entity.UpdatedOn = DateTime.UtcNow;

                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Updated Appointment: Comments = {model.Comments}, AppointmentId = {entity.AppointmentId} ";
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

        public async Task<ApiResponse<SearchReply<AppointmentResponseModel>>> GetList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "patientNumber" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<AppointmentResponseModel>>();


                IQueryable<AppointmentResponseModel> query = (from app in context.NHS_Appointments
                                                              join pat in context.NHS_Patients on app.PatientId equals pat.PatientId
                                                              where app.Deleted == false
                                                              select new AppointmentResponseModel
                                                              {
                                                                  AppointmentId = app.AppointmentId,
                                                                  AppTypeId = app.AppTypeId,
                                                                  StatusId = app.StatusId,
                                                                  SpecialityId = app.SpecialtyId,
                                                                  BookDate = app.BookDate,
                                                                  AppDate = app.AppDate,
                                                                  AppTime = app.AppTime,
                                                                  ConsultantId = app.ConsultantId,
                                                                  HospitalId = app.HospitalId,
                                                                  WardId = app.WardId,
                                                                  DepartmentId = app.DepartmentId,
                                                                  PatientId = app.PatientId,
                                                                  PatientValidationId = app.PatientValidationId,
                                                                  Comments = app.Comments,
                                                                  AppointmentStatus = app.AppointmentStatus,
                                                                  CancellationReason = app.CancellationReason,
                                                                  PatientNumber = pat.DistrictNumber,
                                                                  PatientName = $"{pat.FirstName} {pat.MiddleName} {pat.LastName}",
                                                                  Speciality = context.Specialties.FirstOrDefault(x => x.SpecialtyId == app.SpecialtyId).Name,
                                                                  PatientPathNumber = context.NHS_Patient_Validations.FirstOrDefault(x => x.PatientValidationId == app.PatientValidationId).PathWayNumber
                                                              }).AsQueryable();

                int offset = (pageNumber) * pageSize;

                if (!string.IsNullOrEmpty(options.Parameter.Status))
                {
                    if (options.Parameter.Status == "BOOKED")
                    {
                        query = query.Where(x => x.AppTypeId != 3 || x.BookDate.HasValue || x.AppDate.HasValue);
                    }
                    if (options.Parameter.Status == "PARTIAL")
                    {
                        query = query.Where(x => x.AppTypeId == 3 || !x.BookDate.HasValue || !x.AppDate.HasValue);
                    }
                }
                if (options.Parameter.Id > 0)
                {
                    query = query.Where(x => x.PatientId == options.Parameter.Id);
                }
                switch (sortField)
                {
                    case "patientName":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientName) : query.OrderByDescending(s => s.PatientName);
                        break;
                    case "patientNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientNumber) : query.OrderByDescending(s => s.PatientNumber);
                        break;
                    case "patientPathNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientPathNumber) : query.OrderByDescending(s => s.PatientPathNumber);
                        break;
                    case "appointmentDate":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.AppDate) : query.OrderByDescending(s => s.AppDate);
                        break;
                    case "bookingDate":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.BookDate) : query.OrderByDescending(s => s.BookDate);
                        break;
                    case "speciality":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Speciality) : query.OrderByDescending(s => s.Speciality);
                        break;
                    case "appointmentStatus":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.AppointmentStatus) : query.OrderByDescending(s => s.AppointmentStatus);
                        break;
                      
                    default:
                        query = query.OrderBy(s => s.PatientNumber);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();

                if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                {
                    items = items.Where(x => x.PatientName.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                    || x.Speciality.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                     || x.PatientPathNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                     || x.PatientNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())).ToList();
                }

                var response = new SearchReply<AppointmentResponseModel>()
                {
                    TotalCount = count,
                    Result = items.OrderBy(x=>x.Speciality).ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<AppointmentResponseModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<AppointmentResponseModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<SearchReply<AppointmentResponseModel>>> GetCancelList(SearchCall<SearchParameter> options)
        {
            int count = 0;
            int pageNumber = options.From > 0 ? options.From : 0;
            int pageSize = options.PageSize > 0 ? options.PageSize : 10;
            string sortOrder = string.IsNullOrEmpty(options.SortOrder) ? "asc" : options.SortOrder;
            string sortField = string.IsNullOrEmpty(options.SortField) ? "patientNumber" : options.SortField;

            try
            {
                var apiResponse = new ApiResponse<SearchReply<AppointmentResponseModel>>();


                IQueryable<AppointmentResponseModel> query = (from app in context.NHS_Appointments
                                                              join pat in context.NHS_Patients on app.PatientId equals pat.PatientId
                                                              where app.Deleted == false
                                                              select new AppointmentResponseModel
                                                              {
                                                                  AppointmentId = app.AppointmentId,
                                                                  AppTypeId = app.AppTypeId,
                                                                  StatusId = app.StatusId,
                                                                  SpecialityId = app.SpecialtyId,
                                                                  BookDate = app.BookDate,
                                                                  AppDate = app.AppDate,
                                                                  AppTime = app.AppTime,
                                                                  ConsultantId = app.ConsultantId,
                                                                  HospitalId = app.HospitalId,
                                                                  WardId = app.WardId,
                                                                  DepartmentId = app.DepartmentId,
                                                                  PatientId = app.PatientId,
                                                                  PatientValidationId = app.PatientValidationId,
                                                                  Comments = app.Comments,
                                                                  AppointmentStatus = app.AppointmentStatus,
                                                                  CancellationReason = app.CancellationReason,
                                                                  PatientName = pat.FirstName + " " + pat.MiddleName + " " + pat.LastName,
                                                                  PatientNumber = pat.DistrictNumber,
                                                                  Speciality = context.Specialties.FirstOrDefault(x => x.SpecialtyId == app.SpecialtyId).Name,
                                                                  PatientPathNumber = context.NHS_Patient_Validations.FirstOrDefault(x => x.PatientValidationId == app.PatientValidationId).PathWayNumber
                                                              }).AsQueryable();

                int offset = (pageNumber) * pageSize;

                //if (!string.IsNullOrEmpty(options.Parameter.SearchQuery))
                //{
                //    query = query.Where(x => x.PatientNumber.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                //    || x.Speciality.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower())
                //    || x.AppointmentStatus.Trim().ToLower().Contains(options.Parameter.SearchQuery.Trim().ToLower()));
                //}
                switch (sortField)
                {
                    case "patientNumber":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.PatientNumber) : query.OrderByDescending(s => s.PatientNumber);
                        break;
                    case "speciality":
                        query = sortOrder == "asc" ? query.OrderBy(s => s.Speciality) : query.OrderByDescending(s => s.Speciality);
                        break;

                    default:
                        query = query.OrderBy(s => s.PatientNumber);
                        break;
                }
                count = query.Count();
                var items = await query.Skip(offset).Take(pageSize).ToListAsync();


                var response = new SearchReply<AppointmentResponseModel>()
                {
                    TotalCount = count,
                    Result = items.ToList(),
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<AppointmentResponseModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<AppointmentResponseModel>() { TotalCount = count }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<AppointmentResponseModel>>> Get(int appointmentId)
        {
            try
            {
                if (appointmentId <= 0)
                {
                    return new ApiResponse<GetResponse<AppointmentResponseModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AppointmentResponseModel> { Status = false, Entity = null, Message = "Appointment Id is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<GetResponse<AppointmentResponseModel>>();

                var result = await (from app in context.NHS_Appointments
                                    join pat in context.NHS_Patients on app.PatientId equals pat.PatientId
                                    where app.AppointmentId == appointmentId
                                    select new AppointmentResponseModel
                                    {
                                        AppointmentId = app.AppointmentId,
                                        AppTypeId = app.AppTypeId,
                                        StatusId = app.StatusId,
                                        SpecialityId = app.SpecialtyId,
                                        BookDate = app.BookDate,
                                        AppDate = app.AppDate,
                                        AppTime = app.AppTime,
                                        ConsultantId = app.ConsultantId,
                                        HospitalId = app.HospitalId,
                                        WardId = app.WardId,
                                        DepartmentId = app.DepartmentId,
                                        PatientId = app.PatientId,
                                        PatientValidationId = app.PatientValidationId,
                                        Comments = app.Comments,
                                        AppointmentStatus = app.AppointmentStatus,
                                        CancellationReason = app.CancellationReason,
                                        PatientNumber = pat.DistrictNumber,
                                        Speciality = context.Specialties.FirstOrDefault(x => x.SpecialtyId == app.SpecialtyId).Name,
                                        PatientName = pat.FirstName + " " + pat.MiddleName + " " + pat.LastName,
                                        PatientPathNumber = context.NHS_Patient_Validations.FirstOrDefault(x => x.PatientValidationId == app.PatientValidationId).PathWayNumber
                                    }).FirstOrDefaultAsync();

                if (result == null)
                {
                    return new ApiResponse<GetResponse<AppointmentResponseModel>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<AppointmentResponseModel>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                var response = new GetResponse<AppointmentResponseModel>()
                {
                    Status = true,
                    Entity = result,
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<AppointmentResponseModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<AppointmentResponseModel>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> Delete(int appointmentId)
        {
            try
            {

                if (appointmentId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Appointments.Find(appointmentId);

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

                var details = $"Deleted Appointment: PatientId = {result.PatientId}, Comments = {result.Comments}, BookDate = {result.BookDate} ";
                await auditTrail.SaveAuditTrail(details, "Appointment", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        public async Task<ApiResponse<DeleteReply>> Cancel(int appointmentId)
        {
            try
            {

                if (appointmentId <= 0)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "PatientId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var result = context.NHS_Appointments.Find(appointmentId);

                if (result == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }
                result.AppointmentStatus = "Cancelled";


                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Cancelled Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted Appointment: PatientId = {result.PatientId}, Comments = {result.Comments}, BookDate = {result.BookDate} ";
                await auditTrail.SaveAuditTrail(details, "Appointment", "Cancel");

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
                    var data = await context.NHS_Appointments.FindAsync(item);
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

                var details = $"Deleted Multiple Appointment: with Ids {model.targetIds.ToArray()} ";
                await auditTrail.SaveAuditTrail(details, "Appointment", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

    }
}