using Autofac;
using BetingSystem.Services;

namespace BetingSystem.DevelopmentUtilities
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubCurrentUserAccessor>().As<ICurrentUserAccessor>();
        }
    }
}
