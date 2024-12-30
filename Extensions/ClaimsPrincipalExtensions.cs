using System;
using System.Security.Claims;
//using Microsoft.AspNet.Identity;

namespace Shopsy_Project.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }
    }
}