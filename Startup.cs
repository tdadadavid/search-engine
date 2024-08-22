using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEngine.Contexts;
using SearchEngine.Services;
using SearchEngine.CronJobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

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
            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDBSettings"));

            // Add MongoDBContext
            services.AddSingleton<MongoDBContext>();

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


            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<DocumentIndexingJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DocumentIndexingJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddHostedService<CronService>();


            services.AddScoped<IDocumentService, DocumentService>();

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
