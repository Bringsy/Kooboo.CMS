﻿using System.Security.Claims;
using System.Linq;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Web.Helpers;

namespace Kooboo.CMS.Web.Authorizations
{
    public static class ClaimsPrincipalExtensions
    {
        private static string GetNameClaimType(this ClaimsPrincipal principal)
        {
            if (principal.FindFirst(ClaimTypes.NameIdentifier) != null)
                return ClaimTypes.NameIdentifier;

            if (principal.FindFirst(ClaimTypes.Name) != null)
                return ClaimTypes.Name;

            return ClaimTypes.Email;
        }

        public static ClaimsPrincipal MapToLocalPrincipal(this ClaimsPrincipal principal)
        {
            var identity = new ClaimsIdentity(principal.Claims, "Kooboo",
                GetNameClaimType(principal),
                ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}