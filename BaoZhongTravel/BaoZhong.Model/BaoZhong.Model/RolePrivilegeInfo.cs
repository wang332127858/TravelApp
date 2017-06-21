using System;

namespace BaoZhong.Model
{
    /// <summary>
    /// desc:��ɫȨ����
    /// author:cgm
    /// date:2016/11/8
    /// </summary>
	public class RolePrivilegeInfo : BaseModel
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

		public int Privilege//Ȩ��Id
		{
			get;
			set;
		}

		public long RoleId
		{
			get;
			set;
		}
	}
}
