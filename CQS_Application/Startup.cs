using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Persistence;
using Service;
using Service.Decorators.CommandHandler;
using Service.Decorators.QueryHandler;
using Service.OrderLines.FindOrderLinesByIds;
using Service.Orders.CreateOrder;
using Service.Orders.FindOrderById;
using Service.QueryProcessor;

namespace CqsApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                Database.SetInitializer<CqsDbContext>(
                    new MigrateDatabaseToLatestVersion<CqsDbContext,
                    Persistence.Migrations.Configuration>(useSuppliedContext: true));
                using (var context = new CqsDbContext(Configuration["Data:DefaultConnection:ConnectionString"]))
                {
                    context.Database.Initialize(force: true);
                }
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = true
                    });
                });

            // -- Database Contexts (EF)
            services.AddScoped<ICqsDbContext>(
                (_) => new CqsDbContext(Configuration["Data:DefaultConnection:ConnectionString"]));

            // -- COMPOSITION ROOT (Add IOC Definitions Here)

            /*
             * QUERY HANDLERS 
             * **/

            services.AddTransient<IQueryHandler<FindOrderByIdQuery, Order>>(x =>
                new LogQueryDecorator<FindOrderByIdQuery, Order>(
                    x.GetService<ILogger>(),
                    new LogQueryExceptionDecorator<FindOrderByIdQuery, Order>(
                        x.GetService<ILogger>(),
                        new ValidateQueryDecorator<FindOrderByIdQuery, Order>(
                            new FindOrderByIdQueryHandler(x.GetService<ICqsDbContext>())))));

            services.AddTransient<IQueryHandler<FindOrderLinesByIdsQuery, IEnumerable<OrderLine>>>(x =>
                new LogQueryDecorator<FindOrderLinesByIdsQuery, IEnumerable<OrderLine>>(
                    x.GetService<ILogger>(),
                    new LogQueryExceptionDecorator<FindOrderLinesByIdsQuery, IEnumerable<OrderLine>>(
                        x.GetService<ILogger>(),
                        new ValidateQueryDecorator<FindOrderLinesByIdsQuery, IEnumerable<OrderLine>>(
                            new FindOrderLinesByIdsQueryHandler(x.GetService<ICqsDbContext>())))));

            /*
             * COMMAND HANDLERS   
             * **/

            services.AddTransient<ICommandHandler<CreateOrderCommand>>(x =>
                new LogCommandCalledDecorator<CreateOrderCommand>(
                    x.GetService<ILogger>(),
                    new LogCommandExceptionDecorator<CreateOrderCommand>(
                        x.GetService<ILogger>(),
                        new SaveTransactionDecorator<CreateOrderCommand>(
                            x.GetService<ICqsDbContext>(),
                            x.GetService<ILogger>(),
                            new CreateOrderCommandHandler(
                                x.GetService<ICqsDbContext>(),
                                x.GetService<IQueryProcessor>(),
                                x.GetService<ILogger>())))));

            /*
             * APPLICATION SERVICES
             * **/

            services.AddTransient<IQueryProcessor, QueryProcessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = false // Aurelia Webpack Plugin HMR currently has issues. Leave this set to false.
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
