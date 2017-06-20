using System;
using System.Linq;
using BaoZhong.Model;
using BaoZhong.IServices.QueryModel;

namespace BaoZhong.IServices
{
    public interface ILevelRuleService : IService, IDisposable
    {
        void AddLevelRule(LevelRuleInfo model);

        void UpdateLevelRule(LevelRuleInfo model);

        void DeleteLevelRule(long id);

        IQueryable<LevelRuleInfo> GetLevelRules();
    }
}
