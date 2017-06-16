using BaoZhong.Core.Helper;
using BaoZhong.IServices;
using BaoZhong.Model;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaoZhong.Web.Framework
{
    /// <summary>
    /// desc:管理员控制器基类
    /// author:cgm
    /// date:2016/8/1
    /// </summary>
	public abstract class BaseAdminController : BaseController
    {
        public IPaltManager CurrentManager
        {
            get
            {
                IPaltManager result = null;
                //long num = UserCookieEncryptHelper.Decrypt(WebHelper.GetCookie("BaoZhong-PlatformManager"), "Admin");
                //if (num != 0L)
                //{
                //    result = ServiceHelper.Create<IManagerService>().GetPlatformManager(num);
                //}
                return result;
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.InitVisitorTerminal();
            string text = ConfigurationManager.AppSettings["IsInstalled"];
            if (text != null && !bool.Parse(text))
            {
                return;
            }
            if (filterContext.IsChildAction)
            {
                return;
            }
            if (this.CurrentManager == null)
            {
                if (WebHelper.IsAjax())
                {
                    filterContext.Result = base.Json(new BaseController.Result
                    {
                        msg = "登录超时,请重新登录！",
                        success = false
                    });
                    return;
                }
                RedirectToRouteResult result = base.RedirectToAction("", "Login", new
                {
                    area = "admin"
                });
                filterContext.Result = result;
                return;
            }
            else
            {
                object[] customAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(UnAuthorize), false);
                if (customAttributes == null)
                {
                    return;
                }

                if (customAttributes.Length == 1)
                {
                    return;
                }

                string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                string actionName = filterContext.RouteData.Values["action"].ToString().ToLower();
                if (this.CurrentManager.AdminPrivileges == null || this.CurrentManager.AdminPrivileges.Count == 0 || !AdminPermission.CheckPermissions(this.CurrentManager.AdminPrivileges, controllerName, actionName))
                {
                    if (WebHelper.IsAjax())
                    {
                        filterContext.Result = base.Json(new BaseController.Result
                        {
                            msg = "你没有访问的权限！",
                            success = false
                        });
                        return;
                    }
                    ViewResult viewResult = new ViewResult
                    {
                        ViewName = "NoAccess"
                    };
                    viewResult.TempData.Add("Message", "你没有权限访问此页面");
                    viewResult.TempData.Add("Title", "你没有权限访问此页面！");
                    filterContext.Result = viewResult;
                }
                return;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            filterContext.RouteData.Values["controller"].ToString().ToLower();
            filterContext.RouteData.Values["action"].ToString().ToLower();
        }

        protected ActionResult SuccessfulRedirectView(string viewName, object routeData = null)
        {
            return base.RedirectToAction(viewName, routeData);
        }
    }
}
