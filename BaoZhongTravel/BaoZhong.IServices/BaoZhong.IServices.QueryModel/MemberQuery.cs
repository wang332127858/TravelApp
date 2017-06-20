using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoZhong.IServices.QueryModel
{
    public class MemberQuery :QueryBase
    {
        public string UserId
        {
            get;
            set;
        }

        public string UserName
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

        public string RegisterName
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
