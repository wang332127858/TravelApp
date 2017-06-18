using BaoZhong.Web.Framework;
using System.Web.Mvc;

namespace BaoZhong.Web.Areas.Admin
{
    /// <summary>
    /// desc:管理区域页面路由规则注册
    /// author:cgm
    /// date:2016/8/1
    /// </summary>
	public class AdminAreaRegistration : AreaRegistrationOrder
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override int Order
        {
            get
            {
                return 0;
            }
        }

        public AdminAreaRegistration()
        {
        }

        /// <summary>
        /// desc:注册函数
        /// author:cgm
        /// date:2016/8/1
        /// </summary>
        /// <param name="context">待注册的上下文</param>
		public override void RegisterAreaOrder(AreaRegistrationContext context)
        {
            context.MapRoute("Admin_default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}