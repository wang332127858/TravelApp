using BaoZhong.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BaoZhong.IServices
{
    public interface IGroupService : IService, IDisposable
    {
        void AddGroup(GroupInfo model);

        void UpdateGroup(GroupInfo model);

        int UpdateMarket(GroupInfo model);

        PageModel<GroupInfo> GetGroups(string keyWords, int pageNo, int pageSize, int type);

        IQueryable<GroupInfo> GetGroups();

        IQueryable<GroupInfo> GetAllGroups();

        IQueryable<GroupInfo> GetMarketings();

        bool IsExistGroup(string groupName);

        bool IsExistMarket(string marketName);

        GroupInfo GetGroup(long id);

        //bool GroupInUse(long id);

        void DeleteGroup(long id);
    }
}
