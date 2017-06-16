using System;

namespace BaoZhong.Web.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class UnAuthorize : Attribute
	{
	}
}
