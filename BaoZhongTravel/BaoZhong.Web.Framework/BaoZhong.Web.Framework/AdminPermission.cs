using BaoZhong.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BaoZhong.Web.Framework
{
	public static class AdminPermission
	{
		private static readonly Dictionary<AdminPrivilege, IEnumerable<ActionPermission>> privileges;

		private static readonly IEnumerable<ActionPermission> ActionPermissions;

		public static Dictionary<AdminPrivilege, IEnumerable<ActionPermission>> Privileges
		{
			get
			{
				return AdminPermission.privileges;
			}
		}

		static AdminPermission()
		{
			AdminPermission.ActionPermissions = AdminPermission.GetAllActionByAssembly();
			AdminPermission.privileges = new Dictionary<AdminPrivilege, IEnumerable<ActionPermission>>();
			IEnumerable<List<ActionItem>> enumerable = from a in PrivilegeHelper.GetPrivileges<AdminPrivilege>().Privilege
			select a.Items;
			foreach (List<ActionItem> current in enumerable)
			{
				foreach (ActionItem current2 in current)
				{
					List<ActionPermission> list = new List<ActionPermission>();
					List<Controllers> controllers = current2.Controllers;
					foreach (Controllers current3 in controllers)
					{
						foreach (string current4 in current3.ActionNames)
						{
							IEnumerable<ActionPermission> actionByControllerName = AdminPermission.GetActionByControllerName(current3.ControllerName, current4);
							list.AddRange(actionByControllerName);
						}
					}
					AdminPermission.privileges.Add((AdminPrivilege)current2.PrivilegeId, list);
				}
			}
		}

		private static IEnumerable<ActionPermission> GetActionByControllerName(string controllername, string actionname = "")
		{
			return from item in AdminPermission.ActionPermissions
			where item.ControllerName.ToLower() == controllername.ToLower() && (actionname == "" || item.ActionName.ToLower() == actionname.ToLower())
			select item;
		}

		private static IList<ActionPermission> GetAllActionByAssembly()
		{
			List<ActionPermission> list = new List<ActionPermission>();
			IEnumerable<Type> enumerable = from a in Assembly.Load("BaoZhong.Web").GetTypes()
			where a.BaseType != null && a.BaseType.Name == "BaseAdminController"
			select a;
			foreach (Type current in enumerable)
			{
				MethodInfo[] methods = current.GetMethods();
				MethodInfo[] array = methods;
				for (int i = 0; i < array.Length; i++)
				{
					MethodInfo methodInfo = array[i];
					if (methodInfo.ReturnType.Name == "ActionResult" || methodInfo.ReturnType.Name == "JsonResult")
					{
						ActionPermission actionPermission = new ActionPermission();
						actionPermission.ActionName = methodInfo.Name;
						actionPermission.ControllerName = methodInfo.DeclaringType.Name.Substring(0, methodInfo.DeclaringType.Name.Length - 10);
						object[] customAttributes = methodInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
						if (customAttributes.Length > 0)
						{
							actionPermission.Description = (customAttributes[0] as DescriptionAttribute).Description;
						}
						list.Add(actionPermission);
					}
				}
			}
			return list;
		}

		public static bool CheckPermissions(List<AdminPrivilege> userprivileages, string controllerName, string actionName)
		{
			return userprivileages.Contains((AdminPrivilege)0) || (from a in AdminPermission.privileges
			where userprivileages.Contains(a.Key)
			select a).Any((KeyValuePair<AdminPrivilege, IEnumerable<ActionPermission>> b) => b.Value.Any((ActionPermission c) => c.ControllerName.ToLower() == controllerName.ToLower() && c.ActionName.ToLower() == actionName.ToLower()));
		}
	}
}
