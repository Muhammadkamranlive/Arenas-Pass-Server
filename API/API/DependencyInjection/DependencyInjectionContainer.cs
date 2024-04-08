using Autofac;
using Server.Core;
using Server.BaseService;

namespace API
{
    public class DependencyInjectionContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repo<>)).As(typeof(IRepo<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Base_Service<>)).As(typeof(IBase_Service<>)).InstancePerLifetimeScope();
            

        }
    }
    
}
