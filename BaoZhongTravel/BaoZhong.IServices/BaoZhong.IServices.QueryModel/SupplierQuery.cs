using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoZhong.IServices.QueryModel
{
    public class SupplierQuery :QueryBase
    {
        public string SupplierName
        {
            get;
            set;
        }

        public string TravelName
        {
            get;
            set;
        }

        public string LineName
        {
            get;
            set;
        }

        public string RegionName
        {
            get;
            set;
        }
    }
}
