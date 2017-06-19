using System;
using System.Linq;
using BaoZhong.Model;

namespace BaoZhong.IServices
{
    public interface IPrivilegesService : IService, IDisposable
    {
        void AddPlatformRole(RoleInfo model);

        void AddSellerRole(RoleInfo model);

        void UpdatePlatformRole(RoleInfo model);

        void UpdateSellerRole(RoleInfo model);

        void DeletePlatformRole(long id);

        RoleInfo GetPlatformRole(long id);

        RoleInfo GetSellerRole(long id, long shopId);

        IQueryable<RoleInfo> GetSellerRoles(long shopId);

        IQueryable<RoleInfo> GetPlatformRoles();

        void DeleteSellerRole(long id, long shopId);
    }
}
