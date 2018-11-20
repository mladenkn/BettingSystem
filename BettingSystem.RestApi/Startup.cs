using System;
using System.IO;
using AutoMapper;
using BetingSystem.DAL;
using BetingSystem.DevelopmentUtilities;
using BetingSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace BetingSystem.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddAutoMapper(typeof(MapperProfile).Assembly)
                .AddDbContext<BetingSystemDbContext>(c => c.UseInMemoryDatabase("test-db"))
                .AddScoped<DbContext, BetingSystemDbContext>()
                .AddTransient<IBonusService, BonusService>()
                .AddTransient<ITicketService, TicketService>()
                .AddTransient<IBonusApplier, BonusApplier>()
                .AddTransient<IWalletService, WalletService>()
                .AddTransient<IDataProvider, DataProvider>()
                .AddTransient<ICurrentUserAccessor, StubCurrentUserAccessor>()
                .AddTransient<ITicketBonusesRepository, TicketBonusesRepository>()
                .AddTransient<ISafeRunner, SafeRunner>()
                .AddTransient(_ =>
                {
                    var fileName = Configuration.GetValue<string>("TicketBonusesFile");
                    var filePath = Path.Combine(Environment.ContentRootPath, fileName);
                    return new TicketBonusesRepository.Dependecies { FilePath = filePath };
                })
                ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BetingSystem API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
