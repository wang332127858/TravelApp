using System;
using System.Collections.Generic;

namespace BaoZhong.Core.Plugins
{
	public class FormData
	{
		public class FormItem
		{
			public string Name
			{
				get;
				set;
			}

			public string DisplayName
			{
				get;
				set;
			}

			public bool IsRequired
			{
				get;
				set;
			}

			public FormData.FormItemType Type
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}
		}

		public enum FormItemType
		{
			text = 1,
			checkbox,
			password
		}

		public System.Collections.Generic.IEnumerable<FormData.FormItem> Items
		{
			get;
			set;
		}
	}
}
