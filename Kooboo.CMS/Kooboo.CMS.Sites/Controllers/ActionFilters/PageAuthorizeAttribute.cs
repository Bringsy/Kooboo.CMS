#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class PageAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!Page_Context.Current.Initialized)
            {
                throw new InvalidOperationException();
            }
            var permission = Page_Context.Current.PageRequestContext.Page.Permission;
            if (permission != null)
            {
                return permission.Authorize(httpContext.User);
            }

            return true;
        }

        protected virtual void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            var permission = Page_Context.Current.PageRequestContext.Page.Permission;
            if (permission != null && !string.IsNullOrEmpty(permission.UnauthorizedUrl))
            {
                var unauthorizedUrl = permission.UnauthorizedUrl.AddQueryParam("returnUrl", filterContext.HttpContext.Request.RawUrl);
                var redirectUrl = FrontUrlHelper.WrapperUrl(unauthorizedUrl.TrimStart('~'), Page_Context.Current.PageRequestContext.Site, Page_Context.Current.PageRequestContext.RequestChannel);
                filterContext.Result = new RedirectResult(redirectUrl.ToString());
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
    }
}
