using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Infrastructure;
using SharedModels;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Base URL for the product service when the solution is executed using docker-compose.
        // The product service (running as a container) listens on this URL for HTTP requests
        // from other services specified in the docker compose file (which in this solution is
        // the order service).
        private Uri productServiceBaseUrl = new Uri("http://productapi/api/products/");

        // RabbitMQ connection string (I use CloudAMQP as a RabbitMQ server).
        // Remember to replace this connectionstring with youur own.
        string cloudAMQPConnectionString =
            "host=hawk.rmq.cloudamqp.com;virtualHost=wdedqsoj;username=wdedqsoj;password=GV_TgSrC8n8fiBHEC_VfZ_GoOd-t0I3J";
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In-memory database:
            services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));

            // Register repositories for dependency injection
            services.AddScoped<IRepository<OrderApi.Models.Order>, OrderRepository>();

            // Register database initializer for dependency injection
            services.AddTransient<IDbInitializer, DbInitializer>();

            // Register product service gateway for dependency injection
            services.AddSingleton<IServiceGateway<Product>>(new
                ProductsServiceGateway(productServiceBaseUrl));

            // Register MessagePublisher (a messaging gateway) for dependency injection
            services.AddSingleton<IMessagePublisher>(new
                MessagePublisher(cloudAMQPConnectionString));

            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Initialize the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Initialize the database
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<OrderApiContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
