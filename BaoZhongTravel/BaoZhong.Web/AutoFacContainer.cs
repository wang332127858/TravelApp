using Autofac;
using Autofac.Builder;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using BaoZhong.Core;
using System.Reflection;
using System.Web.Mvc;

namespace BaoZhong.Web
{
	public class AutoFacContainer : IinjectContainer
	{
		private ContainerBuilder builder;

		private IContainer container;

		public AutoFacContainer()
		{
			this.builder = new ContainerBuilder();
			this.SetupResolveRules(this.builder);
			ContainerBuilder containerBuilder = this.builder;
			Assembly[] executingAssembly = new Assembly[] { Assembly.GetExecutingAssembly() };
			containerBuilder.RegisterControllers(executingAssembly);
			this.container = this.builder.Build(ContainerBuildOptions.None);
			DependencyResolver.SetResolver((IDependencyResolver)(new AutofacDependencyResolver(this.container)));
		}

		public void RegisterType<T>()
		{
			this.builder.RegisterType<T>();
		}

		public T Resolve<T>()
		{
			return this.container.Resolve<T>();
		}

		private void SetupResolveRules(ContainerBuilder builder)
		{
            Assembly assembly = Assembly.Load("BaoZhong.IServices");
            Assembly assembly2 = Assembly.Load("BaoZhong.Service");
            (from t in builder.RegisterAssemblyTypes(new Assembly[]
            {
                assembly2,
                assembly
            })
             where t.Name.EndsWith("Service")
             select t).AsImplementedInterfaces<object>();
            ConfigurationSettingsReader module = new ConfigurationSettingsReader("autofac");
            builder.RegisterModule(module);
        }
    }
}