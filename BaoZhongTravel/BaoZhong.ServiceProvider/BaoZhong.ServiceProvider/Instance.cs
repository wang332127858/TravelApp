using BaoZhong.Core;
using BaoZhong.IServices;

namespace BaoZhong.ServiceProvider
{
	public class Instance<T> where T : IService
	{
		public static T Create
		{
			get
			{
				return ObjectContainer.Current.Resolve<T>();
			}
		}
	}
}
