using BaoZhong.Core;
using BaoZhong.Core.Helper;
using BaoZhong.IServices;
using BaoZhong.Model;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaoZhong.Web.Framework
{
    /// <summary>
    /// desc:本平台控制器基类
    /// author:cgm
    /// date:2016/11/8
    /// </summary>
    public abstract class BaseController : Controller
    {
        private int CKLoginTimeOut = 30;

        protected List<JumpUrlRoute> _JumpUrlRouteData
        {
            get;
            set;
        }

        protected bool isCanClearLoginStatus
        {
            get
            {
                bool flag = true;
                DateTime? lastOperateTime = this.LastOperateTime;
                if (lastOperateTime.HasValue)
                {
                    TimeSpan now = DateTime.Now - lastOperateTime.Value;
                    if (now.TotalMinutes <= (double)this.CKLoginTimeOut && now.TotalMinutes >= 0)
                    {
                        flag = false;
                    }
                }
                return flag;
            }
        }

        public List<JumpUrlRoute> JumpUrlRouteData
        {
            get
            {
                return this._JumpUrlRouteData;
            }
        }

        protected DateTime? LastOperateTime
        {
            get
            {
                HttpCookie item = base.HttpContext.Request.Cookies["BaoZhong_LastOpTime"];
                DateTime? nullable = null;
                if (item != null)
                {
                    nullable = new DateTime?(DateTime.FromBinary(long.Parse(item.Value)));
                }
                return nullable;
            }
        }

        public VisitorTerminal visitorTerminalInfo
        {
            get;
            set;
        }

        public BaseController()
        {
            HttpContext current =System.Web.HttpContext.Current;
            if (!this.IsInstalled())
            {
                base.RedirectToAction("/Web/Installer/Agreement");
                return;
            }
        }

        public void ClearDistributionUserLinkId()
        {
            WebHelper.GetCookie("d2cccb104922d434", "");
        }

        private void ClearLoginCookie()
        {
            HttpCookie item = base.HttpContext.Request.Cookies["BaoZhong-User"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
            item = base.HttpContext.Request.Cookies["BaoZhong-SellerManager"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
            item = base.HttpContext.Request.Cookies["BaoZhong_LastOpTime"];
            if (item != null)
            {
                item.Expires = DateTime.Now.AddYears(-1);
                base.HttpContext.Response.AppendCookie(item);
            }
        }

        private void DisposeService(ControllerContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            List<IService> item = filterContext.HttpContext.Session["_serviceInstace"] as List<IService>;
            if (item != null)
            {
                foreach (IService service in item)
                {
                    try
                    {
                        service.Dispose();
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        Log.Error(string.Concat(service.GetType().ToString(), "释放失败！"), exception);
                    }
                }
                filterContext.HttpContext.Session["_serviceInstace"] = null;
            }
        }

        protected Exception GerInnerException(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex;
            }
            return this.GerInnerException(ex.InnerException);
        }

        public List<long> GetDistributionUserLinkId()
        {
            List<long> nums = new List<long>();
            string cookie = WebHelper.GetCookie("d2cccb104922d434");
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                string[] strArrays = cookie.Split(new char[] { ',' });
                long num = (long)0;
                string[] strArrays1 = strArrays;
                for (int i = 0; i < (int)strArrays1.Length; i++)
                {
                    if (long.TryParse(strArrays1[i], out num) && num > (long)0)
                    {
                        nums.Add(num);
                    }
                }
            }
            return nums;
        }

        public JumpUrlRoute GetRouteUrl(string controller, string action, string area, string url)
        {
            JumpUrlRoute item;
            string lower = controller;
            string str = action;
            string lower1 = area;
            this.InitJumpUrlRoute();
            JumpUrlRoute jumpUrlRoute = null;
            url = url.ToLower();
            lower = lower.ToLower();
            str = str.ToLower();
            lower1 = lower1.ToLower();
            List<JumpUrlRoute> jumpUrlRouteData = this.JumpUrlRouteData;
            if (!string.IsNullOrWhiteSpace(lower1))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Area.ToLower() == lower1);
            }
            if (!string.IsNullOrWhiteSpace(lower))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Controller.ToLower() == lower);
            }
            if (!string.IsNullOrWhiteSpace(str))
            {
                jumpUrlRouteData = jumpUrlRouteData.FindAll((JumpUrlRoute d) => d.Action.ToLower() == str);
            }
            if (jumpUrlRouteData.Count > 0)
            {
                item = jumpUrlRouteData[0];
            }
            else
            {
                item = null;
            }
            jumpUrlRoute = item;
            if (jumpUrlRoute == null)
            {
                JumpUrlRoute jumpUrlRoute1 = new JumpUrlRoute()
                {
                    PC = url,
                    WAP = url,
                    WX = url
                };
                jumpUrlRoute = jumpUrlRoute1;
            }
            return jumpUrlRoute;
        }

        public void InitJumpUrlRoute()
        {
            this._JumpUrlRouteData = new List<JumpUrlRoute>();
            JumpUrlRoute jumpUrlRoute = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "UserOrder",
                PC = "/userorder",
                WAP = "/member/orders",
                WX = "/member/orders"
            };
            this._JumpUrlRouteData.Add(jumpUrlRoute);
            JumpUrlRoute jumpUrlRoute1 = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "UserCenter",
                PC = "/usercenter",
                WAP = "/member/center",
                WX = "/member/center"
            };
            this._JumpUrlRouteData.Add(jumpUrlRoute1);
            JumpUrlRoute jumpUrlRoute2 = new JumpUrlRoute()
            {
                Action = "Index",
                Area = "Web",
                Controller = "Login",
                PC = "/login",
                WAP = "/login/entrance",
                WX = "/login/entrance"
            };
            this._JumpUrlRouteData.Add(jumpUrlRoute2);
            JumpUrlRoute jumpUrlRoute3 = new JumpUrlRoute()
            {
                Action = "Home",
                Area = "Web",
                Controller = "Shop",
                PC = "/shop",
                WAP = "/vshop/detail",
                WX = "/vshop/detail",
                IsSpecial = true
            };
            this._JumpUrlRouteData.Add(jumpUrlRoute3);
            JumpUrlRoute jumpUrlRoute4 = new JumpUrlRoute()
            {
                Action = "Submit",
                Area = "Web",
                Controller = "Order",
                PC = "/order/submit",
                WAP = "/order/SubmiteByCart",
                WX = "/order/SubmiteByCart",
                IsSpecial = true
            };
            this._JumpUrlRouteData.Add(jumpUrlRoute4);
        }

 
        protected void InitVisitorTerminal()
        {
            VisitorTerminal visitorTerminal = new VisitorTerminal();
           
            //目前只保证PC端的访问
            visitorTerminal.Terminal = EnumVisitorTerminal.PC;
            this.visitorTerminalInfo = visitorTerminal;
        }

        private bool IsExistPage(string url)
        {
            bool flag = false;
            HttpWebResponse uRLResponse = WebHelper.GetURLResponse(url, "get", "", null, 20000);
            if (uRLResponse != null && (uRLResponse.StatusCode == HttpStatusCode.OK || uRLResponse.StatusCode == HttpStatusCode.Found || uRLResponse.StatusCode == HttpStatusCode.MovedPermanently))
            {
                flag = true;
            }
            return flag;
        }

        private bool IsInstalled()
        {
            string item = ConfigurationManager.AppSettings["IsInstalled"];
            if (item == null)
            {
                return true;
            }
            return bool.Parse(item);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.InitVisitorTerminal();
            if (!this.IsInstalled() && filterContext.RouteData.Values["controller"].ToString().ToLower() != "admin")
            {
                filterContext.Result = new RedirectResult("/common/site/close");
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = this.GerInnerException(filterContext.Exception);
            string message = exception.Message;
            base.OnException(filterContext);
            if (!(exception is BaoZhongException))
            {
                string str = filterContext.RouteData.Values["controller"].ToString();
                string str1 = filterContext.RouteData.Values["action"].ToString();
                object item = filterContext.RouteData.DataTokens["area"];
                string str2 = string.Format("页面未捕获的异常：Area:{0},Controller:{1},Action:{2}", item, str, str1);
                Log.Error(str2, exception);
                message = "系统内部异常";
            }
            if (!WebHelper.IsAjax())
            {
                ViewResult viewResult = new ViewResult()
                {
                    ViewName = "Error"
                };
                viewResult.TempData.Add("Message", filterContext.Exception.ToString());
                viewResult.TempData.Add("Title", message);
                filterContext.Result = viewResult;
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                this.DisposeService(filterContext);
            }
            else
            {
                BaseController.Result result = new BaseController.Result()
                {
                    success = false,
                    msg = message,
                    status = -9999
                };
                filterContext.Result = base.Json(result);
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                this.DisposeService(filterContext);
            }
            if (exception is HttpRequestValidationException)
            {
                if (!WebHelper.IsAjax())
                {
                    ContentResult contentResult = new ContentResult()
                    {
                        Content = "<script src='/Scripts/jquery-1.11.1.min.js'></script>"
                    };
                    ContentResult contentResult1 = contentResult;
                    contentResult1.Content = string.Concat(contentResult1.Content, "<script src='/Scripts/jquery.artDialog.js'></script>");
                    ContentResult contentResult2 = contentResult;
                    contentResult2.Content = string.Concat(contentResult2.Content, "<script src='/Scripts/artDialog.iframeTools.js'></script>");
                    ContentResult contentResult3 = contentResult;
                    contentResult3.Content = string.Concat(contentResult3.Content, "<link href='/Content/artdialog.css' rel='stylesheet' />");
                    ContentResult contentResult4 = contentResult;
                    contentResult4.Content = string.Concat(contentResult4.Content, "<link href='/Content/bootstrap.min.css' rel='stylesheet' />");
                    ContentResult contentResult5 = contentResult;
                    contentResult5.Content = string.Concat(contentResult5.Content, "<script>$(function(){$.dialog.errorTips('您提交了非法字符！',function(){window.history.back(-1)},2);});</script>");
                    filterContext.Result = contentResult;
                }
                else
                {
                    BaseController.Result result1 = new BaseController.Result()
                    {
                        msg = "您提交了非法字符!"
                    };
                    filterContext.Result = base.Json(result1);
                }
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.ExceptionHandled = true;
                this.DisposeService(filterContext);
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void SaveDistributionUserLinkId(long partnerid, long shopid, long uid)
        {
            //if (partnerid > (long)0 && shopid > (long)0)
            //{
            //    long num = (long)0;
            //    num = ServiceHelper.Create<IMemberService>().UpdateShareUserId(uid, partnerid, shopid);
            //    List<long> distributionUserLinkId = this.GetDistributionUserLinkId();
            //    if (num > (long)0)
            //    {
            //        distributionUserLinkId.Add(num);
            //    }
            //    if (distributionUserLinkId.Count > 0)
            //    {
            //        WebHelper.SetCookie("d2cccb104922d434", string.Join<long>(",", distributionUserLinkId.ToArray()));
            //        return;
            //    }
            //    this.ClearDistributionUserLinkId();
            //}
        }

        protected void SetLastOperateTime(DateTime? date = null)
        {
            if (!date.HasValue)
            {
                date = new DateTime?(DateTime.Now);
            }
            HttpCookie item = base.HttpContext.Request.Cookies["BaoZhong_LastOpTime"];
            DateTime.Now.AddYears(-1);
            if (item != null)
            {
                DateTime.FromBinary(long.Parse(item.Value));
            }
            else
            {
                item = new HttpCookie("BaoZhong_LastOpTime");
            }
            item.Value = date.Value.Ticks.ToString();
            base.HttpContext.Response.AppendCookie(item);
        }

        public class Result
        {
            public string msg
            {
                get;
                set;
            }

            public int status
            {
                get;
                set;
            }

            public bool success
            {
                get;
                set;
            }

            public Result()
            {
            }
        }
    }
}