using System;
using System.Collections.Generic;

namespace BaoZhong.Model
{
	public interface ISellerManager : IManager
	{
		List<SellerPrivilege> SellerPrivileges
		{
			get;
			set;
		}
	}
}
