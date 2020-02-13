//using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreTestProject.log;
using NetCoreTestProject.Repositories;
using NLog;
using System.IO;

namespace NetCoreTestProject
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static string SendGridAPIKey = string.Empty;
        public static string MessageQueue = string.Empty;

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // database migration in package manager console
        //1. : Add-Migration NetCoreTestProject.Repositories.TestDBContext or dotnet ef migrations add "add field" --project "c:\Project\NetCoreTestProject\"
        //2. : update-database or dotnet ef database update --verbose --project "c:\Project\NetCoreTestProject\"
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();
            // set nlog
            services.AddSingleton<ILog, LogNLog>();

            //read database connection string
            services.AddDbContext<TestWorkDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //read send grid api key
            SendGridAPIKey = Configuration.GetSection("SendGridAPIKey").Value;

            //read MessageQueue setting
            MessageQueue = Configuration.GetSection("MessageQueue").Value;
            services.AddScoped<ITestRepository, TestRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
