using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoZhong.Model
{
    public class MemberInfo : BaseModel
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

        public string UserId
        {
            get;
            set;
        }

        public long UserLevel
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public long Sex
        {
            get;
            set;
        }

        public string CardId
        {
            get;
            set;
        }

        public DateTime BornDate
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public string PassportId
        {
            get;
            set;
        }

        public DateTime PassportDate
        {
            get;
            set;
        }

        public string PassportAddr
        {
            get;
            set;
        }

        public string CellPhone
        {
            get;
            set;
        }

        public string TelePhone
        {
            get;
            set;
        }

        public string RecommendUserId
        {
            get;
            set;
        }

        public string RecommendUserName
        {
            get;
            set;
        }

        public long TopRegionId
        {
            get;
            set;
        }

        public long RegionId
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string RegisterName
        {
            get;
            set;
        }

        public long Points
        {
            get;
            set;
        }

        public string Health
        {
            get;
            set;
        }

        public string Interest
        {
            get;
            set;
        }

        public string Profession
        {
            get;
            set;
        }

        public string InterestLine
        {
            get;
            set;
        }
    }
}
