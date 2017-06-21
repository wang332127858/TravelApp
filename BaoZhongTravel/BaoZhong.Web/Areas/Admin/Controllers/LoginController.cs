using BaoZhong.Core;
using BaoZhong.Core.Helper;
using BaoZhong.IServices;
using BaoZhong.Model;
using BaoZhong.Web.Framework;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace BaoZhong.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// desc:平台登录控制器
    /// author:cgm
    /// date:2016/8/1
    /// </summary>
	public class LoginController : BaseController
    {
        private const int TIMES_WITHOUT_CHECKCODE = 3;

        private IManagerService _iManagerService;

        public LoginController(IManagerService iManagerService)
        {
            this._iManagerService = iManagerService;
        }

        private void CheckCheckCode(string username, string checkCode)
        {
            if (this.GetErrorTimes(username) >= 3)
            {
                if (string.IsNullOrWhiteSpace(checkCode))
                {
                    throw new BaoZhongException("30分钟内登录错误3次以上需要提供验证码");
                }
                if ((base.Session["checkCode"] as string).ToLower() != checkCode.ToLower())
                {
                    throw new BaoZhongException("验证码错误");
                }
                base.Session["checkCode"] = Guid.NewGuid().ToString();
            }
        }

        [HttpPost]
        public JsonResult CheckCode(string checkCode)
        {
            JsonResult jsonResult;
            try
            {
                string item = base.Session["checkCode"] as string;
                bool lower = item.ToLower() == checkCode.ToLower();
                jsonResult = base.Json(new { success = lower });
            }
            catch (BaoZhongException BaoZhongException1)
            {
                BaoZhongException BaoZhongException = BaoZhongException1;
                jsonResult = base.Json(new { success = false, msg = BaoZhongException.Message });
            }
            catch (Exception exception)
            {
                Log.Error("检验验证码时发生异常", exception);
                jsonResult = base.Json(new { success = false, msg = "未知错误" });
            }
            return jsonResult;
        }

        private void CheckInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new BaoZhongException("请填写用户名");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new BaoZhongException("请填写密码");
            }
        }

        private void ClearErrorTimes(string username)
        {
            Cache.Remove(CacheKeyCollection.ManagerLoginError(username));
        }

        public ActionResult GetCheckCode()
        {
            string str;
            MemoryStream memoryStream = ImageHelper.GenerateCheckCode(out str);
            base.Session["checkCode"] = str;
            return base.File(memoryStream.ToArray(), "image/png");
        }

        [HttpPost]
        public JsonResult GetErrorLoginTimes(string username)
        {
            return base.Json(new { errorTimes = this.GetErrorTimes(username) });
        }

        private int GetErrorTimes(string username)
        {
            object obj = Cache.Get(CacheKeyCollection.ManagerLoginError(username));
            return (obj == null ? 0 : int.Parse(obj.ToString()));
        }

        public ActionResult Index()
        {
            string item = ConfigurationManager.AppSettings["IsInstalled"];
            if (item == null || bool.Parse(item))
            {
                return base.View();
            }
            return base.RedirectToAction("Agreement", "Installer", new { area = "Web" });
        }

        [HttpPost]
        public JsonResult Login(string username, string password, string checkCode)
        {
            JsonResult jsonResult;
            try
            {
                this.CheckInput(username, password);
                this.CheckCheckCode(username, checkCode);
                ManagerInfo man = this._iManagerService.GetPlatformManagerByName(username);
                if (man == null)
                {
                    throw new BaoZhongException("该用户不存在!");
                }
                ManagerInfo managerInfo = this._iManagerService.Login(username, password);
                if (managerInfo == null)
                {
                    throw new BaoZhongException("用户名和密码不匹配");
                }

                this.ClearErrorTimes(username);
                jsonResult = base.Json(new { success = true, userId = UserCookieEncryptHelper.Encrypt(managerInfo.Id, "Admin") });
            }
            catch (BaoZhongException BaoZhongException1)
            {
                BaoZhongException BaoZhongException = BaoZhongException1;
                int num = this.SetErrorTimes(username);
                jsonResult = base.Json(new { success = false, msg = BaoZhongException.Message, errorTimes = num, minTimesWithoutCheckCode = 3 });
            }
            catch (Exception exception2)
            {
                Exception exception = exception2;
                int num1 = this.SetErrorTimes(username);
                Exception exception1 = base.GerInnerException(exception);
                string message = "未知错误";
                if (!(exception1 is BaoZhongException))
                {
                    Log.Error(string.Concat("用户", username, "登录时发生异常"), exception);
                }
                else
                {
                    message = exception1.Message;
                }
                jsonResult = base.Json(new { success = false, msg = message, errorTimes = num1, minTimesWithoutCheckCode = 3 });
            }
            return jsonResult;
        }

        private int SetErrorTimes(string username)
        {
            int errorTimes = this.GetErrorTimes(username) + 1;
            string str = CacheKeyCollection.ManagerLoginError(username);
            DateTime now = DateTime.Now;
            Cache.Insert<int>(str, errorTimes, now.AddMinutes(30));
            return errorTimes;
        }
    }
}