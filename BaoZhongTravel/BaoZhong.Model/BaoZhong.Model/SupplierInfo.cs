using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaoZhong.Model
{
    public class SupplierInfo :BaseModel
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

        public string SupplierId
        {
            get;
            set;
        }

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

        public string LeaderName
        {
            get;
            set;
        }

        public string LeaderPhone
        {
            get;
            set;
        }

        public string SellerName
        {
            get;
            set;
        }

        public long ContactWay
        {
            get;
            set;
        }


        public string SellerContact
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }



        public string TravelAddress
        {
            get;
            set;
        }

        public string LineId
        {
            get;
            set;
        }

        public string LineName
        {
            get;
            set;
        }

        public string RegionId
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
