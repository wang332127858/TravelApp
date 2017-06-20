using BaoZhong.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BaoZhong.Entity
{
    /// <summary>
    /// desc:������
    /// author:cgm
    /// date:2016/11/8
    /// </summary>
	public class Entities : DbContext
	{
        /// <summary>
        /// ���¼�������������ĸΪ��A-Z
        /// </summary>

        public virtual DbSet<ManagerInfo> ManagerInfo
        {
            get;
            set;
        }

        public virtual DbSet<RolePrivilegeInfo> RolePrivilegeInfo
        {
            get;
            set;
        }

        public virtual DbSet<RoleInfo> RoleInfo
        {
            get;
            set;
        }
        public virtual DbSet<GroupInfo> GroupInfo
        {
            get;
            set;
        }

        public virtual DbSet<MemberInfo> MemberInfo
        {
            get;
            set;
        }

        public virtual DbSet<LevelRuleInfo> LevelRuleInfo
        {
            get;
            set;
        }

        public virtual DbSet<SupplierInfo> SupplierInfo
        {
            get;
            set;
        }

        public Entities() : base("name=Entities")
		{
			
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			throw new UnintentionalCodeFirstException();
		}
	}
}
