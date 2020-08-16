using System;
using System.Reflection;
using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rixtrema.BLL.Handlers.Implementations;
using Rixtrema.BLL.Handlers.Interfaces;
using Rixtrema.DAL.EF.Contexts;
using Rixtrema.DAL.EF.Repositories.Implementations;
using Rixtrema.DAL.EF.Repositories.Interfaces;
using Rixtrema.DAL.EF.Services.Interfaces;
using Module = Autofac.Module;

namespace Rixtrema.WebApi
{
    public class Startup
    {
        private const string AppSettings = "AppSettings";
        private const string AppSettingsFile = "appsettings.json";

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(AppSettingsFile,  true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            Configuration = builder.Build();
        }
        

        public IConfiguration Configuration { get; }
        
        public ILifetimeScope AutofacContainer { get; private set; }
        
        private class AppSettingsModel
        {
            public string ConnectionString { set; get; }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOptions();
            
            var settings = new AppSettingsModel();
            Configuration.Bind(AppSettings, settings);
            
            // // ReSharper disable once CommentTypo
            // // Add Autofac.
            // var containerBuilder = new ContainerBuilder();
            // containerBuilder.RegisterModule<DefaultModule>();
            // containerBuilder.Register(icc => settings).SingleInstance();
            // containerBuilder.Populate(services);
            //
            // // Presenters.
            // containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //     .Where(t => t.Name.EndsWith(Presenter)).SingleInstance();
            //
            // ApplicationContainer = containerBuilder.Build();
        }
        
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder) {
            // Register your own things directly with Autofac
            builder.RegisterType<BaseContext>();
            builder.RegisterType<IDbService>();

            builder.RegisterType<PercentileHandler>().As<IPercentileHandler>();
            
            builder.RegisterType<PercentileRepository>().As<IPercentileRepository>();
            builder.RegisterType<StateRepository>().As<IStateRepository>();
            builder.RegisterType<PlansRepository>().As<IPlansRepository>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            //this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
        }
        
        
    }
}