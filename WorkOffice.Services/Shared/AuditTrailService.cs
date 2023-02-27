using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Contracts.Models;
using WorkOffice.Contracts.ServicesContracts.Shared;
using WorkOffice.Domain.Entities;
using WorkOffice.Domain.Helpers;
using WorkOffice.Common.Interfaces;
using WorkOffice.Contracts.Models.Shared;

namespace WorkOffice.Services.Shared
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly DataContext _context;
        private readonly IHttpAccessorService httpAccessorService;
        private readonly ILoggerService _loggerService;

        public AuditTrailService(DataContext appContext, IHttpAccessorService httpAccessorService, ILoggerService loggerService)
        {
            _context = appContext;
            this.httpAccessorService = httpAccessorService;
            _loggerService = loggerService;
        }

        public async Task SaveAuditTrail(string details, string page, string actionType)
        {
            try
            {
                var auditTrail = new AuditTrail
                {
                    Details = details,
                    ActionDate = DateTime.UtcNow,
                    ActionBy = httpAccessorService.GetCurrentUserName() ?? null,
                    IPAddress = httpAccessorService.GetClientIP() ?? null,
                    HostAddress = httpAccessorService.GetHostAddress() ?? null,
                    Page = page,
                    ActionType = actionType,
                };
                _context.AuditTrails.Add(auditTrail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _loggerService.Error($"[AuditTrailService] Saving [{details}] ->  failed  {ex}");
                _loggerService.Error($"[AuditTrailService] Saving [{details}] ->  failed  {ex.InnerException.Message}");
                _loggerService.Error($"[AuditTrailService] Saving [{details}] ->  failed  {ex.InnerException.InnerException.Message}");

                throw new Exception(ex.Message);
            }
        }

        public async Task SaveNotification(List<SaveNotificationModel> notificationModels)
        {
            try
            {
                List<Notification> notifications = new List<Notification>();
                foreach (var item in notificationModels)
                {
                    var notification = new Notification
                    {
                        SenderId = item.SenderId,
                        ReceiverId = item.ReceiverId,
                        Title = item.Title,
                        Body = item.Body,
                        IsRead = false
                    };
                    notifications.Add(notification);
                }
                if (notifications.Count > 0)
                {
                    _context.Notifications.AddRange(notifications);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _loggerService.Error($"[SaveNotification] Saving  ->  failed  {ex}");
                _loggerService.Error($"[SaveNotification] Saving  ->  failed  {ex.InnerException.Message}");
                _loggerService.Error($"[SaveNotification] Saving  ->  failed  {ex.InnerException.InnerException.Message}");

                throw new Exception(ex.Message);
            }

        }

        public async Task<ApiResponse<SearchReply<SearchAuditTrailModel>>> SearchAuditTrail(SearchCall<string> options)
        {
            try
            {
                var apiResponse = new ApiResponse<SearchReply<SearchAuditTrailModel>>();
                var query = _context.AuditTrails.Select(u => new SearchAuditTrailModel
                {
                    Details = u.Details,
                    ActionDate = u.ActionDate,
                    ActionBy = u.ActionBy,
                    IPAddress = u.IPAddress,
                    HostAddress = u.HostAddress,
                    Page = u.Page,
                    ActionType = u.ActionType,
                });

                if (!string.IsNullOrEmpty(options.Parameter))
                {
                    query = query.Where(x => x.Details.Contains(options.Parameter));
                }
                var result = await query.OrderByDescending(x => x.ActionDate).Take(options.PageSize).Skip(options.From).ToListAsync();
                var response = new SearchReply<SearchAuditTrailModel>()
                {
                    TotalCount = query.Count(),
                    Result = result,
                    Errors = null
                };

                apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                apiResponse.IsSuccess = true;
                apiResponse.ResponseType = response;

                return apiResponse;

            }
            catch (Exception ex)
            {
                return new ApiResponse<SearchReply<SearchAuditTrailModel>>() { StatusCode = System.Net.HttpStatusCode.BadRequest, ResponseType = new SearchReply<SearchAuditTrailModel>() { TotalCount = 0, Result = null, Errors = null }, IsSuccess = false };
            }
        }

    }
}
