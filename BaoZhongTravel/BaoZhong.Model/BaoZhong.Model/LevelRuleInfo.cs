using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoZhong.Model
{
    public class LevelRuleInfo : BaseModel
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

        public long UserLevel
        {
            get;
            set;
        }

        public long BeginPoint
        {
            get;
            set;
        }

        public long EndPoint
        {
            get;
            set;
        }
    }
}
