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
    public class MembersService : ServiceBase, IMembersService, IService, IDisposable
    {
        public PageModel<MemberInfo> GetMembers(MemberQuery query)
        {
            IQueryable<MemberInfo> disable = this.context.MemberInfo.AsQueryable();

            int total = 0;
            //IQueryable<SupplierInfo> models = disable.FindBy( query.PageNo, query.PageSize, out total, (SupplierInfo item) => item.RoleId, false);
            return new PageModel<MemberInfo>
            {
                //Models = models,
                Total = total
            };
        }

        public MemberInfo GetMember(string userId)
        {
            return this.context.MemberInfo.FindBy((MemberInfo item) => item.UserId == userId).FirstOrDefault();
        }

        public void UpdateMemberLevel(string userId, long level)
        {
            MemberInfo updatemodel = this.context.MemberInfo.FindBy((MemberInfo a) => a.UserId == userId).FirstOrDefault<MemberInfo>();
            if (updatemodel == null)
            {
                throw new BaoZhongException("找不到该会员");
            }

            updatemodel.UserLevel = level;
            this.context.SaveChanges();
        }

        public void DeleteMember(long id)
        {
            MemberInfo entity = (from a in this.context.MemberInfo
                                 where a.Id == id
                                   select a).FirstOrDefault<MemberInfo>();
            this.context.MemberInfo.Remove(entity);
            this.context.SaveChanges();
        }
    }
}
