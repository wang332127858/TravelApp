using System.ComponentModel.DataAnnotations.Schema;

namespace BaoZhong.Model
{
	public class SiteSettingsInfo : BaseModel
	{
        [NotMapped]
        public string UserCookieKey
        {
            get;
            set;
        }

        [NotMapped]
        public string DivideDateKey
        {
            get;
            set;
        }
        [NotMapped]
        public string Version
        {
            get;
            set;
        }
        [NotMapped]
        public string AppUrl
        {
            get;
            set;
        }
        [NotMapped]
        public string UpdateDesc
        {
            get;
            set;
        }
        [NotMapped]
        public string ForceUpdate
        {
            get;
            set;
        }

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

		internal string Key
		{
			get;
			set;
		}

		internal string Value
		{
			get;
			set;
		}
	}
}
