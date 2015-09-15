using System;
using System.Diagnostics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Extensions.ExecutionContextScoping;
using RateDocker.Repositories;
using Couchbase;
using System.Collections.Generic;
using Couchbase.Configuration.Client;

namespace RateDocker
{
    public class Startup
    {
        private Container container = new SimpleInjector.Container();
        
        private ClientConfiguration couchbaseConfiguration;
        
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC services to the services container.
            services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
            
            services.AddInstance<IControllerActivator>(new SimpleInjectorControllerActivator(this.container));
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
            
            couchbaseConfiguration = new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri(Configuration.GetSection("AppSettings:DatabaseUrl").Value)
                }
            };
            
            InitializeContainer(app);
            RegisterControllers(app);
    
            container.Verify(VerificationOption.VerifyAndDiagnose);
    
            // Wrap requests in a execution context scope. This allows
            // scoped instances to be resolved from the container.
            app.Use(async (context, next) => {
                using (container.BeginExecutionContextScope()) {
                    await next();
                }
            });
        }
        
        private void InitializeContainer(IApplicationBuilder app)
        {
            // For instance: 
            container.Register<IVotingRepository, VotingRepository>();
            container.Register<Cluster>(() => new Cluster(couchbaseConfiguration), Lifestyle.Singleton);
        }
    
        private void RegisterControllers(IApplicationBuilder app)
        {
            // Register ASP.NET controllers
            var provider = app.ApplicationServices.GetRequiredService<IControllerTypeProvider>();
            foreach (var type in provider.ControllerTypes)
            {
                var registration = Lifestyle.Transient.CreateRegistration(type, container);
                container.AddRegistration(type, registration);
                registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, 
                    "ASP.NET disposes controllers.");
            }
        }
    }
    internal sealed class SimpleInjectorControllerActivator : IControllerActivator {
        private readonly Container container;
        public SimpleInjectorControllerActivator(Container container) { this.container = container; }
    
        [DebuggerStepThrough]
        public object Create(ActionContext context, Type controllerType) {
            return container.GetInstance(controllerType);
        }
    }
}
