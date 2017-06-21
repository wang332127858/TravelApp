using System;
using System.Linq;
using BaoZhong.Model;

namespace BaoZhong.IServices
{
    public interface IPrivilegesService : IService, IDisposable
    {
        void AddPlatformRole(RoleInfo model);

        void UpdatePlatformRole(RoleInfo model);

        void DeletePlatformRole(long id);

        RoleInfo GetPlatformRole(long id);

        IQueryable<RoleInfo> GetPlatformRoles();
    }
}
