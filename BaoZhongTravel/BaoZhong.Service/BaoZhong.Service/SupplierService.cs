using BaoZhong.Core;
using BaoZhong.Core.Helper;
using BaoZhong.Entity;
using BaoZhong.IServices;
using BaoZhong.IServices.QueryModel;
using BaoZhong.Model;
using BaoZhong.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace BaoZhong.Service
{
    public class SupplierService : ServiceBase, ISupplierService, IService, IDisposable
    {
        public PageModel<SupplierInfo> GetSuppliers(SupplierQuery query)
        {
            IQueryable<SupplierInfo> disable = this.context.SupplierInfo.AsQueryable();
            
            int total = 0;
            //IQueryable<SupplierInfo> models = disable.FindBy( query.PageNo, query.PageSize, out total, (SupplierInfo item) => item.RoleId, false);
            return new PageModel<SupplierInfo>
            {
                //Models = models,
                Total = total
            };
        }

        public SupplierInfo GetSupplier(string userId)
        {
            return this.context.SupplierInfo.FindBy((SupplierInfo item) => item.SupplierId == userId ).FirstOrDefault();
        }

        //void UpdateMemberLevel(long userId, long level);

        public void DeleteSupplier(long id)
        {
            SupplierInfo entity = (from a in this.context.SupplierInfo
                                    where a.Id == id
                                    select a).FirstOrDefault<SupplierInfo>();
            this.context.SupplierInfo.Remove(entity);
            this.context.SaveChanges();
        }
    }
}
