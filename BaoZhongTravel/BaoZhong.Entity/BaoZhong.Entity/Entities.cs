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

        public Entities() : base("name=Entities")
		{
			
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			throw new UnintentionalCodeFirstException();
		}
	}
}
