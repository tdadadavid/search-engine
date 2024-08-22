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
using SearchEngine.Api.Core.Files;

namespace SearchEngine
{
    /// <summary>
    /// Configures services and the app's request pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for the application.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// Configures the services used by the application.
        /// </summary>
        /// <param name="services">The collection of services to add to the container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure MongoDB settings
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDBSettings"));

            // Add MongoDBContext
            services.AddSingleton<MongoDBContext>();
            services.AddSingleton<CloudStoreManager>();
            services.AddSingleton<FileManager>();
      services.AddSingleton<DocumentService>();

      services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                // var connectionString = Configuration.GetConnectionString("MongoDBSettings:ConnectionString");
                return new MongoClient("mongodb://localhost:27017/NebularFinder");
        //          var mongoDBSettings = sp.GetRequiredService<MongoDBSettings>();
        // return new MongoClient(mongoDBSettings.ConnectionString);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("search");
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
                cronExpression: "0/5 * * * * ?"));
            services.AddHostedService<CronService>();
            services.Configure<FormOptions>(options =>
            {
              options.MultipartBodyLengthLimit = 52428800;
            });


            services.AddScoped<IDocumentService, DocumentService>();

            // Add MVC controllers
            services.AddControllers();
        }

        /// <summary>
        /// Configures the application's request pipeline.
        /// </summary>
        /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                     app.UseStaticFiles();
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
