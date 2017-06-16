using System.Collections.Generic;

namespace BaoZhong.Model
{
	public class Privileges
	{
		public List<GroupActionItem> Privilege
		{
			get;
			set;
		}

		public Privileges()
		{
			this.Privilege = new List<GroupActionItem>();
		}
	}
}
