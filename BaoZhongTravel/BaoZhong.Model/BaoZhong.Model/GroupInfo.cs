using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoZhong.Model
{
    public class GroupInfo : BaseModel
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

        public string GroupName
        {
            get;
            set;
        }

        public int Description
        {
            get;
            set;
        }
    }
}
