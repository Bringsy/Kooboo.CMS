#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using System.Security.Principal;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Authorizations
{
    public static class AuthorizationHelpers
    {
        public static bool Authorize(this RequestContext requestContext, Permission permission)
        {
            IPrincipal user = requestContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            var site = GetSite(requestContext);

            return Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Authorize(site, user.Identity.Name, permission);
        }

        private static Site GetSite(RequestContext requestContext)
        {
            var siteName = requestContext.GetRequestValue("siteName");
            if (!string.IsNullOrEmpty(siteName))
            {
                return SiteHelper.Parse(siteName);
            }
            return null;
        }
    }
}