using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace EAuction.Infrastructure.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class EAuctionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers["authorize-key"].Equals("eauctionsecret321"))
                return;
            else
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        }
    }
}