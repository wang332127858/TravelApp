using BaoZhong.Entity;

namespace BaoZhong.Service
{
	public class ServiceBase
	{
		protected Entities context;

		public ServiceBase()
		{
			this.context = new Entities();
		}

		public void Dispose()
		{
			if (this.context != null)
			{
				this.context.Dispose();
			}
		}
	}
}
