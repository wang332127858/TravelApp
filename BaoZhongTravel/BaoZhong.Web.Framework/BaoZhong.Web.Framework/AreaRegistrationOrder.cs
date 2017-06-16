using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BaoZhong.Web.Framework
{
	public abstract class AreaRegistrationOrder : AreaRegistration
	{
		protected static List<AreaRegistrationContext> areaContent = new List<AreaRegistrationContext>();

		protected static List<AreaRegistrationOrder> areaRegistration = new List<AreaRegistrationOrder>();

		public abstract int Order
		{
			get;
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			AreaRegistrationOrder.areaContent.Add(context);
			AreaRegistrationOrder.areaRegistration.Add(this);
		}

		public abstract void RegisterAreaOrder(AreaRegistrationContext context);

		public static void RegisterAllAreasOrder()
		{
			AreaRegistration.RegisterAllAreas();
			AreaRegistrationOrder.Register();
		}

		private static void Register()
		{
			List<int[]> list = new List<int[]>();
			for (int i = 0; i < AreaRegistrationOrder.areaRegistration.Count; i++)
			{
				list.Add(new int[]
				{
					AreaRegistrationOrder.areaRegistration[i].Order,
					i
				});
			}
			list = (from o in list
			orderby o[0]
			select o).ToList<int[]>();
			foreach (int[] current in list)
			{
				AreaRegistrationOrder.areaRegistration[current[1]].RegisterAreaOrder(AreaRegistrationOrder.areaContent[current[1]]);
			}
		}
	}
}
