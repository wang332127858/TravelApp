using System;
using System.Linq;
using BaoZhong.Model;
using BaoZhong.IServices.QueryModel;

namespace BaoZhong.IServices
{
    public interface IMembersService : IService, IDisposable
    {
        PageModel<MemberInfo> GetMembers(MemberQuery query);

        MemberInfo GetMember(string userId);

        void UpdateMemberLevel(string userId,long level);

        void DeleteMember(long id);
    }
}
