
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

namespace H2RHRMS.Core.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly DataContext context;
        private readonly IAuditTrailService auditTrail;

        public AdministrationService(DataContext appContext, IAuditTrailService _auditTrail)
        {
            context = appContext;
            this.auditTrail = _auditTrail;
        }

        #region User Role
        public async Task<ApiResponse<CreateResponse>> AddUpdateUserRoleAndActivity(UserRoleAndActivityModel model)
        {
            try
            {

                if (model.ClientId == Guid.Empty)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "Request is not coming from a valid client" }, IsSuccess = false };
                }
                if (string.IsNullOrEmpty(model.UserRoleDefinition))
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = "UserRoleDefinition is required." }, IsSuccess = false };
                }
                var isExist = UserRoleDefinitionExists(model.UserRoleDefinition, model.UserRoleAndActivityId);
                if (isExist)
                {
                    return new ApiResponse<CreateResponse>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new CreateResponse() { Status = false, Id = "", Message = $"{model.UserRoleDefinition} already exist." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<CreateResponse>();
                bool result = false;
                UserRoleDefinition definition = null;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<UserRoleActivity> activities = new List<UserRoleActivity>();

                        if (model.activities.Count > 0)
                        {
                            foreach (var item in model.activities)
                            {
                                var newActivity = new UserRoleActivity()
                                {
                                    UserActivityId = item.UserActivityId,
                                    CanAdd = item.CanAdd,
                                    CanEdit = item.CanEdit,
                                    CanApprove = item.CanApprove,
                                    CanDelete = item.CanDelete,
                                    CanView = item.CanView,
                                    ClientId = model.ClientId
                                };
                                activities.Add(newActivity);
                            }
                        }

                        if (model.UserRoleAndActivityId != Guid.Empty)
                        {
                            definition = context.UserRoleDefinitions.Find(model.UserRoleAndActivityId);

                            if (definition != null)
                            {
                                var targetActivities = context.UserRoleActivities.Where(x => x.UserRoleDefinitionId == definition.UserRoleDefinitionId).ToList();

                                if (targetActivities.Any())
                                {
                                    context.UserRoleActivities.RemoveRange(targetActivities);
                                }
                                definition.RoleName = model.UserRoleDefinition;
                                definition.UserRoleActivity = activities;
                                definition.ClientId = model.ClientId;
                            }
                        }
                        else
                        {
                            definition = new UserRoleDefinition
                            {
                                RoleName = model.UserRoleDefinition,
                                ClientId = model.ClientId,
                                UserRoleActivity = activities,
                            };
                            context.UserRoleDefinitions.Add(definition);
                        }
                        result = await context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            var details = $"Created new UserRoleDefinition: UserRoleDefinition = {model.UserRoleDefinition} ";
                           await auditTrail.SaveAuditTrail(details, "UserRoleDefinition", "Create");
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
                    Id = definition.UserRoleDefinitionId,
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

        public async Task<ApiResponse<GetResponse<List<UserRoleDefinitionModel>>>> GetAllUserRoleDefinitions(Guid clientId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<UserRoleDefinitionModel>>>();

                var roles = await (from a in context.UserRoleDefinitions
                                       //join b in context.UserAccountRoles on a.Id equals b.UserRoleDefinitionId
                                       //into grp
                                       //from b in grp.DefaultIfEmpty()
                                   where a.IsDeleted == false && a.ClientId == clientId
                                   orderby a.RoleName
                                   select new UserRoleDefinitionModel
                                   {
                                       UserRoleDefinitionId = a.UserRoleDefinitionId,
                                       RoleName = a.RoleName,
                                       //    UserCount = grp.Count()
                                   }).ToListAsync();



                if (roles.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<UserRoleDefinitionModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<UserRoleDefinitionModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<UserRoleDefinitionModel>>()
                {
                    Status = true,
                    Entity = roles.GroupBy(x => x.UserRoleDefinitionId).Select(k => k.FirstOrDefault()).ToList(),
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<UserRoleDefinitionModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<UserRoleDefinitionModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<DeleteReply>> DeleteUserRoleDefinition(Guid userRoleDefinitionId)
        {
            try
            {
                if (userRoleDefinitionId == Guid.Empty)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply { Status = false, Message = "UserRoleDefinitionId is required." }, IsSuccess = false };
                }

                var apiResponse = new ApiResponse<DeleteReply>();

                var definition = await context.UserRoleDefinitions.FindAsync(userRoleDefinitionId);
                var activities = context.UserRoleActivities.Where(x => x.UserRoleDefinitionId == userRoleDefinitionId).ToList();
                if (definition == null)
                {
                    return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new DeleteReply { Status = false, Message = "No record found" }, IsSuccess = false };
                }

                definition.IsDeleted = true;
                if (activities.Count > 0)
                {
                    context.UserRoleActivities.RemoveRange(activities);
                }

                var response = new DeleteReply()
                {
                    Status = await context.SaveChangesAsync() > 0,
                    Message = "Record Deleted Successfully"
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                var details = $"Deleted UserRoleDefinition: RoleName = {definition.RoleName}";
                await auditTrail.SaveAuditTrail(details, "UserRoleDefinition", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }


        public async Task<ApiResponse<DeleteReply>> DeleteMultipleUserRoleDefinition(MultipleDeleteModel model)
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
                    var user = context.UserRoleDefinitions.Find(item);
                    var activities = context.UserRoleActivities.Where(x => x.UserRoleDefinitionId == item).ToList();
                    if (user != null)
                    {
                        user.IsDeleted = true;
                        if (activities.Count > 0)
                        {
                            context.UserRoleActivities.RemoveRange(activities);
                        }
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

                var details = $"Deleted Multiple UserRoleDefinition: with Ids {model.targetIds.ToArray()} ";
               await  auditTrail.SaveAuditTrail(details, "UserRoleDefinition", "Delete");

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<DeleteReply>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new DeleteReply() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public bool UserRoleDefinitionExists(string roleName, Guid userRoleDefinitionId)
        {
            try
            {
                if (context.UserRoleDefinitions.Any(x => x.RoleName.ToLower().Trim() == roleName.ToLower().Trim() && x.IsDeleted == false && x.UserRoleDefinitionId != userRoleDefinitionId))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region User Role Activities

        public async Task<ApiResponse<GetResponse<List<UserActivityParentModel>>>> GetActivities(Guid clientId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<UserActivityParentModel>>>();

                var result = await (from p in context.UserActivityParents
                                    where p.ClientId == clientId
                                    select new UserActivityParentModel
                                    {
                                        UserActivityParentId = p.UserActivityParentId,
                                        UserActivityParentName = p.UserActivityParentName,
                                        activities = context.UserActivities
                                                       .Where(x => x.UserActivityParentId == p.UserActivityParentId && x.ClientId == clientId)
                                                       .Select(x => new UserActivityModel
                                                       {
                                                           UserActivityId = x.UserActivityId,
                                                           UserActivityName = x.UserActivityName,
                                                           UserActivityParentId = x.UserActivityParentId
                                                       }).ToList()
                                    }).ToListAsync();

                if (result.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<UserActivityParentModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<UserActivityParentModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<UserActivityParentModel>>()
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
                return new ApiResponse<GetResponse<List<UserActivityParentModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<UserActivityParentModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>> GetUserRoleAndActivities(Guid clientId, Guid userRoleId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>();
                List<UserRoleActivitiesModel> activitiesModels = new List<UserRoleActivitiesModel>();
                var data = await (from p in context.UserActivityParents
                                  join q in context.UserActivities on p.UserActivityParentId equals q.UserActivityParentId
                                  orderby p.UserActivityParentName
                                  where p.ClientId == clientId
                                  select new UserRoleActivitiesModel
                                  {
                                      UserRoleDefinitionId = Guid.NewGuid(),
                                      UserRoleDefinition = "",
                                      UserActivityParentId = p.UserActivityParentId,
                                      UserActivityParentName = p.UserActivityParentName,
                                      UserActivityId = q.UserActivityId,
                                      UserActivityName = q.UserActivityName,
                                      CanAdd = false,
                                      CanApprove = false,
                                      CanDelete = false,
                                      CanEdit = false,
                                      CanView = false
                                  }).ToListAsync();

                if (userRoleId != Guid.Empty)
                {
                    var data2 = await (from a in context.UserRoleDefinitions
                                       join b in context.UserRoleActivities on a.UserRoleDefinitionId equals b.UserRoleDefinitionId
                                       join q in context.UserActivities on b.UserActivityId equals q.UserActivityId
                                       where b.UserRoleDefinitionId == userRoleId && b.ClientId == clientId
                                       orderby q.UserActivityParent.UserActivityParentName
                                       select new UserRoleActivitiesModel
                                       {
                                           UserRoleDefinitionId = a.UserRoleDefinitionId,
                                           UserRoleDefinition = a.RoleName,
                                           UserActivityParentId = q.UserActivityParentId,
                                           UserActivityParentName = q.UserActivityParent.UserActivityParentName,
                                           UserActivityId = q.UserActivityId,
                                           UserActivityName = q.UserActivityName,
                                           CanAdd = (bool)b.CanAdd,
                                           CanApprove = (bool)b.CanApprove,
                                           CanDelete = (bool)b.CanDelete,
                                           CanEdit = (bool)b.CanEdit,
                                           CanView = (bool)b.CanView
                                       }).ToListAsync();
                    if (data2.Any())
                    {
                        var data2Ids = data2.Select(a => a.UserActivityId).ToList();

                        var result = data2.Concat(data.Where(x => !data2Ids.Contains(x.UserActivityId)));

                        activitiesModels = result.OrderByDescending(x => x.UserRoleDefinitionId).ToList();
                    }
                    else
                    {
                        activitiesModels = data;
                    }

                }

                if (activitiesModels.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<UserRoleActivitiesModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<UserRoleActivitiesModel>>()
                {
                    Status = true,
                    Entity = activitiesModels,
                    Message = ""
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse<GetResponse<List<UserRoleActivitiesModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<UserRoleActivitiesModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        public async Task<ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>> GetActivitiesByRoleId(Guid clientId, Guid userRoleId)
        {
            try
            {
                var apiResponse = new ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>();

                var result = await context.UserRoleActivities.Where(a => a.UserRoleDefinitionId == userRoleId && a.ClientId == clientId)
                .Select(x => new UserActivitiesByRoleModel
                {
                    UserActivityId = x.UserActivityId,
                    UserRoleDefinitionId = x.UserRoleDefinitionId
                }).ToListAsync();

                if (result.Count <= 0)
                {
                    return new ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>() { StatusCode = System.Net.HttpStatusCode.NotFound, ResponseType = new GetResponse<List<UserActivitiesByRoleModel>>() { Status = false, Message = "No record found" }, IsSuccess = false };
                }


                var response = new GetResponse<List<UserActivitiesByRoleModel>>()
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
                return new ApiResponse<GetResponse<List<UserActivitiesByRoleModel>>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new GetResponse<List<UserActivitiesByRoleModel>>() { Status = false, Message = $"Error encountered {ex.Message}" }, IsSuccess = false };
            }
        }

        #endregion
    }
}
