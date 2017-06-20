using System;
using System.Linq;
using BaoZhong.Model;
using BaoZhong.IServices.QueryModel;

namespace BaoZhong.IServices
{
    public interface ISupplierService : IService, IDisposable
    {
        PageModel<SupplierInfo> GetSuppliers(SupplierQuery query);

        SupplierInfo GetSupplier(string userId);

        //void UpdateMemberLevel(long userId, long level);

        void DeleteSupplier(long id);
    }
}
