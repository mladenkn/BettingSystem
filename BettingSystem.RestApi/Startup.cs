using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BetingSystem.DAL;
using BetingSystem.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "BetingSystem API", Version = "v1" });
            });

            return ServiceProviderFactory.Create(services, c => c.Register(GetBonusesRepoDeps));
        }

        private TicketBonusesRepository.Dependecies GetBonusesRepoDeps(IComponentContext _)
        {
            var fileName = Configuration.GetValue<string>("TicketBonusesFile");
            return new TicketBonusesRepository.Dependecies
            {
                FilePath = Path.Combine(Environment.ContentRootPath, fileName)
            };
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
