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
    public class LevelRuleService : ServiceBase, ILevelRuleService, IService, IDisposable
    {
        public void AddLevelRule(LevelRuleInfo model)
        {
            bool flag = this.context.LevelRuleInfo.Any((LevelRuleInfo a) => a.UserLevel == model.UserLevel);
            if (flag)
            {
                throw new BaoZhongException("已配置相同会员级别");
            }
            this.context.LevelRuleInfo.Add(model);
            this.context.SaveChanges();
        }

        public void UpdateLevelRule(LevelRuleInfo model)
        {
            LevelRuleInfo updatemodel = this.context.LevelRuleInfo.FindBy((LevelRuleInfo a) => a.Id == model.Id && a.UserLevel == model.UserLevel).FirstOrDefault<LevelRuleInfo>();
            if (updatemodel == null)
            {
                throw new BaoZhongException("找不到该会员级别");
            }
         
            updatemodel.BeginPoint = model.BeginPoint;
            updatemodel.EndPoint = model.EndPoint;
            this.context.SaveChanges();
        }

        public void DeleteLevelRule(long id)
        {
            LevelRuleInfo entity = (from a in this.context.LevelRuleInfo
                                    where a.Id == id 
                               select a).FirstOrDefault<LevelRuleInfo>();
            this.context.LevelRuleInfo.Remove(entity);
            this.context.SaveChanges();
        }

        public IQueryable<LevelRuleInfo> GetLevelRules()
        {
            return this.context.LevelRuleInfo.FindAll();
        }
    }
}
