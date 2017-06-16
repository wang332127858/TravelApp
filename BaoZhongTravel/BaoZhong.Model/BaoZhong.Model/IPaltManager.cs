using System.Collections.Generic;

namespace BaoZhong.Model
{
	public interface IPaltManager : IManager
	{
		List<AdminPrivilege> AdminPrivileges
		{
			get;
			set;
		}
	}
}
