
namespace BaoZhong.Core
{
	public interface IinjectContainer
	{
		void RegisterType<T>();

		T Resolve<T>();
	}
}
