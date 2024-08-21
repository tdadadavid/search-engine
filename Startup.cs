using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEngine.Contexts;
using SearchEngine.Api.Core.Services;
using SearchEngine.CronJobs;
using SearchEngine.Api.Core.Interfaces;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using search.SearchEngine.Api.Core.Cronjobs;
using MongoDB.Driver;

namespace SearchEngine
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure MongoDB settings
            services.Configure<MongoDBSettings>(Configuration.GetSection("ConnectionStrings:MongoDb"));

            // Add MongoDBContext
            services.AddSingleton<MongoDBContext>();
            services.AddSingleton<CloudStoreManager>();

            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var connectionString = Configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("search"); // Replace with your database name
            });

            // Add Document Service
            services.AddSingleton<IDocumentService, DocumentService>();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add Quartz.NET services
            // Add Quartz.NET services
            services.AddSingleton<IJobFactory, JobFactory>(); // Ensure you have a JobFactory implementation
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<DocumentIndexingJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DocumentIndexingJob),
                cronExpression: "0 0/5 * * * ?")); // Every 5 minutes
            services.AddHostedService<CronService>();
            services.Configure<FormOptions>(options =>
            {
              options.MultipartBodyLengthLimit = 52428800;
            });

            // Add MVC controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
