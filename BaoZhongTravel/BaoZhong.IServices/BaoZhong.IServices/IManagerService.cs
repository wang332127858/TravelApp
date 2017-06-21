using System;
using System.Linq;
using BaoZhong.Model;
using BaoZhong.IServices.QueryModel;

namespace BaoZhong.IServices
{
    public interface IManagerService : IService, IDisposable
    {
        PageModel<ManagerInfo> GetPlatformManagers(ManagerQuery query);


        ManagerInfo GetPlatformManager(long userId);

        ManagerInfo GetManager(long userId, bool isPlatFormManager = true);


        ManagerInfo Login(string username, string password);

        void Logout(string username);
        ManagerInfo GetPlatformManagerByName(string name);

        void ChangePlatformManagerPassword(long id, string password, long roleId);
        bool ChangePassword(string username, string password);

        long AddPlatformManager(ManagerInfo model);
        void DeletePlatformManager(long id);
        void BatchDeletePlatformManager(long[] ids);
        IQueryable<ManagerInfo> GetPlatformManagerByRoleId(long roleId);

        bool CheckUserNameExist(string userName, bool isPlatFormManager = false);

        

        IQueryable<ManagerInfo> GetManagers(string keyWords);

        long GetManagersId(string UserName);

        ManagerInfo GetManagerInfoByName(string name);
    }
}
