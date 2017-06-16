using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BaoZhong.Core;
using BaoZhong.Web.Framework;

namespace BaoZhong.Web
{
    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
        }

        protected void Application_End()
        {
            string curDomainUrl = SiteStaticInfo.CurDomainUrl;
            if (!string.IsNullOrWhiteSpace(curDomainUrl))
            {
                HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(curDomainUrl)).GetResponse();
            }
        }

        protected void Application_Start()
        {
            //note default infos
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("Areas/");
            //RegistAtStart.Regist();//暂不开放插件注册
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //GlobalConfiguration.Configure(new Action<HttpConfiguration>(WebApiConfig.Register));//暂不开放对外WebAPI接口
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistrationOrder.RegisterAllAreasOrder();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ObjectContainer.ApplicationStart(new AutoFacContainer());
        }
    }
}