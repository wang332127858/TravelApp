using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoZhong.Model
{
	public class ManagerInfo : BaseModel
	{
		private long _id;

		public new long Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				base.Id = value;
			}
		}

        public long ShopId
		{
			get;
			set;
		}

		public long RoleId
		{
			get;
			set;
		}

        public long GroupId
        {
            get;
            set;
        }

        public long ParentId
        {
            get;
            set;
        }

        public string ParentName
        {
            get;
            set;
        }

        public string UserName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string PasswordSalt
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

        public int IsLogin
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string BankAccountName
        {
            get;
            set;
        }

        public string BankAccountNumber
        {
            get;
            set;
        }
	}
}
