using BaoZhong.Core.Helper;
using BaoZhong.IServices;
using BaoZhong.Model;
using BaoZhong.Web.Framework;
using System.Configuration;
using System.Web.Mvc;

namespace BaoZhong.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// desc:平台管理主页控制器
    /// author:cgm
    /// date:2016/11/9
    /// </summary>
	public class HomeController : BaseAdminController
	{
		//private IManagerService _iManagerService;
  //      private IShopService _iShopService;
		public HomeController(/*IManagerService iManagerService, IShopService iShopservice*/)
		{
			//this._iManagerService = iManagerService;
   //         this._iShopService = iShopservice;
		}

        /// <summary>
        /// desc:
        /// </summary>
        /// <returns></returns>
		[UnAuthorize]
		public ActionResult About()
		{
			return base.View();
		}

		[HttpPost]
		[UnAuthorize]
		public JsonResult ChangePassword(string oldpassword, string password)
		{
			if (string.IsNullOrWhiteSpace(oldpassword) || string.IsNullOrWhiteSpace(password))
			{
				BaseController.Result result = new BaseController.Result()
				{
					success = false,
					msg = "密码不能为空！"
				};
				return base.Json(result);
			}
			//IPaltManager currentManager = base.CurrentManager;
			//if (SecureHelper.MD5(string.Concat(SecureHelper.MD5(oldpassword), currentManager.PasswordSalt)) != currentManager.Password)
			//{
			//	BaseController.Result result1 = new BaseController.Result()
			//	{
			//		success = false,
			//		msg = "旧密码错误"
			//	};
			//	return base.Json(result1);
			//}
			//this._iManagerService.ChangePlatformManagerPassword(currentManager.Id, password, (long)0);
			BaseController.Result result2 = new BaseController.Result()
			{
				success = true,
				msg = "修改成功"
			};
			return base.Json(result2);
		}

		[UnAuthorize]
		public JsonResult CheckOldPassword(string password)
		{
			//IPaltManager currentManager = base.CurrentManager;
			//string str = SecureHelper.MD5(string.Concat(SecureHelper.MD5(password), currentManager.PasswordSalt));
			//if (currentManager.Password == str)
			//{
			//	return base.Json(new BaseController.Result()
			//	{
			//		success = true
			//	});
			//}
			return base.Json(new BaseController.Result()
			{
				success = false
			});
		}

		[UnAuthorize]
		public ActionResult Console()
		{
            //return base.View(this._iShopService.GetPlatConsoleMode());
            return base.View();
		}

		[UnAuthorize]
		public ActionResult Copyright()
		{
			return base.View();
		}

		[HttpGet]
		[UnAuthorize]
		public ActionResult GetRecentMonthShopSaleRankChart()
		{
            //LineChartDataModel<int> recentMonthShopSaleRankChart = this._iStatisticsService.GetRecentMonthShopSaleRankChart();
            //return base.Json(new { successful = true, chart = recentMonthShopSaleRankChart }, JsonRequestBehavior.AllowGet);
            return base.Json("");

        }

		[HttpGet]
		[UnAuthorize]
		public ActionResult Index()
		{
            string str = ConfigurationManager.AppSettings["IsInstalled"];
    //        if ((str != null) && !bool.Parse(str))
    //        {
    //            return base.RedirectToAction("Agreement", "Installer", new { area = "Web" });
    //        }
    //((dynamic)base.ViewBag).Name = base.CurrentManager.UserName;
    //        ((dynamic)base.ViewBag).Rights = string.Join<int>(",", from a in base.CurrentManager.AdminPrivileges
    //                                                               select (int)a into a
    //                                                               orderby a
    //                                                               select a);
            return base.View();
        }

		[HttpGet]
		[UnAuthorize]
		public ActionResult ProductRecentMonthSaleRank()
		{
            //LineChartDataModel<int> recentMonthSaleRankChart = this._iStatisticsService.GetRecentMonthSaleRankChart();
            //return base.Json(new { successful = true, chart = recentMonthSaleRankChart }, JsonRequestBehavior.AllowGet);

            return base.Json("");
        }

        [HttpGet]
        [UnAuthorize]
        public JsonResult Logout()
        {
           // this._iManagerService.Logout(this.CurrentManager.UserName);
            return base.Json(new { success = true });
        }

    }
}