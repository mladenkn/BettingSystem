using Autofac;
using Autofac.Extensions.DependencyInjection;
using BetingSystem.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.Infrastructure
{
    public static class ServiceContainerFactory
    {
        public static ILifetimeScope Create(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule<Module>();
            builder.RegisterModule<DAL.Module>();
            builder.RegisterModule<DevelopmentUtilities.Module>();

            builder.RegisterType<TicketBonusesAccessor>().As<ITicketBonusesAccessor>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
