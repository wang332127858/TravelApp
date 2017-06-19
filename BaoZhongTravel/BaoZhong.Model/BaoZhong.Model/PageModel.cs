using System;
using System.Linq;

namespace BaoZhong.Model
{
	public class PageModel<T>
	{
		public IQueryable<T> Models
		{
			get;
			set;
		}

		public int Total
		{
			get;
			set;
		}
	}
	public class PageModel<T, TData>
	{
		public IQueryable<T> Models
		{
			get;
			set;
		}

		public int Total
		{
			get;
			set;
		}

		public TData TotalData
		{
			get;
			set;
		}
	}
}
