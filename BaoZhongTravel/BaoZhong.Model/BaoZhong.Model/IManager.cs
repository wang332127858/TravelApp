using System;

namespace BaoZhong.Model
{
	public interface IManager
	{
		long Id
		{
			get;
			set;
		}

		long ShopId
		{
			get;
			set;
		}

        long GroupId
        {
            get;
            set;
        }

		long RoleId
		{
			get;
			set;
		}

        long RecommendId
        {
            get;
            set;
        }

		string UserName
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}

		string PasswordSalt
		{
			get;
			set;
		}

		DateTime CreateDate
		{
			get;
			set;
		}

		string Description
		{
			get;
			set;
		}

		string RoleName
		{
			get;
			set;
		}

        int IsLogin
        {
            get;
            set;
        }
    }
}
