using System;

namespace BaoZhong.Model
{
    /// <summary>
    /// desc:权限属性基类
    /// author:cgm
    /// date:2016/11/8
    /// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class PrivilegeAttribute : Attribute
	{
		public string GroupName
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Controller
		{
			get;
			set;
		}

		public string Action
		{
			get;
			set;
		}

		public int Pid
		{
			get;
			set;
		}

		public PrivilegeAttribute(string groupName, string name, int pid, string url, string controller, string action = "")
		{
			this.Name = name;
			this.GroupName = groupName;
			this.Pid = pid;
			this.Url = url;
			this.Controller = controller;
			this.Action = action;
		}

		public PrivilegeAttribute(string controller, string action = "")
		{
			this.Controller = controller;
			this.Action = action;
		}
	}
}
