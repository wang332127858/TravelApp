using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoZhong.IServices.QueryModel
{
    public class ManagerQuery : QueryBase
    {
        public long ShopID
        {
            get;
            set;
        }

        public long userID
        {
            get;
            set;
        }

        public long? GroupId
        {
            get;
            set;
        }

        public long RoleId
        {
            get;
            set;
        }

        public string keyWords
        {
            get;
            set;
        }

        public string ParentName
        {
            get;
            set;
        }

        public bool IsMarket
        {
            get;
            set;
        }
    }
}
