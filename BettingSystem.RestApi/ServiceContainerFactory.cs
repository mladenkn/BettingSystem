using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.RestApi
{
    public static class ServiceContainerFactory
    {
        public static ILifetimeScope Create(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<Module>();
            builder.RegisterModule<DAL.Module>();
            return builder.Build();
        }
    }
}
