using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace BaoZhong.Web.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class GZipAttribute : ActionFilterAttribute
	{
		private bool isEnableCompression = true;

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			object[] customAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoCompress), false);
			object[] customAttributes2 = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoCompress), false);
			if (customAttributes2.Length == 1 || customAttributes.Length == 1)
			{
				this.isEnableCompression = false;
			}
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (filterContext.Exception != null || !this.isEnableCompression)
			{
				return;
			}
			HttpResponseBase response = filterContext.HttpContext.Response;
			if (response.Filter is GZipStream || response.Filter is DeflateStream)
			{
				return;
			}
			string text = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower();
				if (text.Contains("gzip") && response.Filter != null)
				{
					response.AppendHeader("Content-Encoding", "gzip");
					response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
					return;
				}
				if (text.Contains("deflate") && response.Filter != null)
				{
					response.AppendHeader("Content-Encoding", "deflate");
					response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
				}
			}
		}
	}
}
