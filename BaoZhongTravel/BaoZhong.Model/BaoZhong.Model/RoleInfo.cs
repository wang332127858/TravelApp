using System;
using System.Collections.Generic;

namespace BaoZhong.Model
{

    public class RoleInfo : BaseModel
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

        public string RoleName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public virtual ICollection<RolePrivilegeInfo> RolePrivilegeInfo
        {
            get;
            set;
        }

        public RoleInfo()
        {
            this.RolePrivilegeInfo = new HashSet<RolePrivilegeInfo>();
        }
    }
}
