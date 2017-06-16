
namespace BaoZhong.Core
{
	public class ObjectContainer
	{
		private static ObjectContainer current;

		private static IinjectContainer container;

		public static ObjectContainer Current
		{
			get
			{
				if (ObjectContainer.current == null)
				{
					ObjectContainer.ApplicationStart(ObjectContainer.container);
				}
				return ObjectContainer.current;
			}
		}

		protected IinjectContainer Container
		{
			get;
			set;
		}

		public static void ApplicationStart(IinjectContainer c)
		{
			ObjectContainer.container = c;
			ObjectContainer.current = new ObjectContainer(ObjectContainer.container);
		}

		protected ObjectContainer()
		{
			this.Container = new DefaultContainerForDictionary();
		}

		protected ObjectContainer(IinjectContainer inversion)
		{
			this.Container = inversion;
		}

		public void RegisterType<T>()
		{
			this.Container.RegisterType<T>();
		}

		public T Resolve<T>()
		{
			return this.Container.Resolve<T>();
		}
	}
}
