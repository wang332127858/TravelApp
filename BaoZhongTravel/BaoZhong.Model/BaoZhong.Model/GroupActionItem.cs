using System.Collections.Generic;

namespace BaoZhong.Model
{
	public class GroupActionItem
	{
		public string GroupName
		{
			get;
			set;
		}

		public List<ActionItem> Items
		{
			get;
			set;
		}

		public GroupActionItem()
		{
			this.Items = new List<ActionItem>();
		}
	}
}
