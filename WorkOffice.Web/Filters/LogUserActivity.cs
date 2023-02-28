using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkOffice.Contracts.ServicesContracts;

namespace WorkOffice.Web.Filters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = Guid.Parse(resultContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = (IUserAccountService)resultContext.HttpContext.RequestServices.GetService(typeof(IUserAccountService));
            await repo.UpdateLastActive(userId);
        }
    }
}
