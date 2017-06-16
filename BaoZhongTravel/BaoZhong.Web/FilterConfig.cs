using BaoZhong.Web.Framework;
using System.Web.Mvc;

namespace BaoZhong.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GZipAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
