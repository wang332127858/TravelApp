using System;
using System.Linq;
using BaoZhong.Model;
using BaoZhong.IServices.QueryModel;

namespace BaoZhong.IServices
{
    public interface IManagerService : IService, IDisposable
    {
        PageModel<ManagerInfo> GetPlatformManagers(ManagerQuery query);

        PageModel<ManagerInfo> GetGroupMembers(ManagerQuery query);

        PageModel<ManagerInfo> GetBusinessManagers(ManagerQuery query);

        IQueryable<ManagerInfo> GetMarketMembers();

        IQueryable<ManagerInfo> GetAgentsOrChannels(long parentId);

        IQueryable<ManagerInfo> GetAgents();

        ManagerInfo GetPlatformManager(long userId);

        void UpdatePlatformManagerRegion(long ManId, string Region);

        ManagerInfo GetManager(long userId, bool isPlatFormManager = true);
        ManagerInfo GetManagerByShopId(long shopId);

        ManagerInfo GetChannelManager(long id);
        ManagerInfo GetPlatformManagerByName(string name);
        ManagerInfo GetBusinessByName(string name);
        ManagerInfo GetManagerInfoByName(string name);

        ManagerInfo Login(string username, string password, bool isPlatFormManager = false);
        bool SetClientId(string username, string ClientId);
        ManagerInfo LoginForMarket(string username, string password);

        void Logout(string username);
        void ChangePlatformManagerPassword(long id, string password, long roleId, string realname, string phone, string BankAccountName, string BankAccountNumber, string Address);
        void ChangeShopManagerPassword(long id, string password);
        void ChangePlatformManagerPassword(long id, string password, long roleId);
        bool ChangePassword(string username, string password);
        void UpdateManagerGroup(long id, long groupId, long parentId);
        int UpdateMarketMember(long id, long parentId, long remark);

        long AddPlatformManager(ManagerInfo model);
        long AddShopManager(ManagerInfo model);

        void DeletePlatformManager(long id);

        void BatchDeletePlatformManager(long[] ids);
  
        bool CheckUserNameExist(string userName, bool isPlatFormManager = false);

        IQueryable<ManagerInfo> GetPlatformManagerByRoleId(long roleId);

        IQueryable<ManagerInfo> GetManagers(string keyWords);

        long GetManagersId(string UserName);
  }
}
