using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BetingSystem.DAL;
using BetingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BetingSystem.RestApi
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider Create(IServiceCollection services, Action<ContainerBuilder> configAdditional)
        {
            var builder = new ContainerBuilder();

            services.AddAutoMapper(typeof(MapperProfile).Assembly);
            services.AddDbContext<BetingSystemDbContext>(c => c.UseInMemoryDatabase("test-db"));

            builder.Populate(services);

            configAdditional(builder);

            builder.RegisterModule<Module>();
            builder.RegisterModule<DAL.Module>();
            builder.RegisterModule<DevelopmentUtilities.Module>();

            builder.RegisterType<TicketBonusesRepository>().As<ITicketBonusesRepository>().SingleInstance();
            
            return new AutofacServiceProvider(builder.Build());
        }
    }
}
