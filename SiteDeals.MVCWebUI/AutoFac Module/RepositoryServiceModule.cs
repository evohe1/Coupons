using Autofac;
using SiteDeals.Core.Repositories;
using SiteDeals.Core.Services;
using SiteDeals.Core.UnitOfWorks;
using SiteDeals.Repository;
using SiteDeals.Repository.Repositories;
using SiteDeals.Repository.UnitOfWorks;
using SiteDeals.Service.Mapping;
using SiteDeals.Service.Service;
using System.Reflection;
using Module = Autofac.Module;
namespace SiteDeals.MVCWebUI.AutoFac_Module

{
    public class RepositoryServiceModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();



            var webAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(webAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(webAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(webAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(webAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsSelf().InstancePerLifetimeScope();




        }
    }
}
