using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Data;
using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cidean.GatherHub
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            //load appsettings from json file(s)
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            //build configuration
            Configuration = builder.Build();
            Environment = environment;

           
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //load typed appsettings as singleton service
            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddMvc();

            services.AddDbContext<HubContext>(options =>
                 options.UseSqlite("Filename=./hub.db"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,  HubContext hubContext, AppSettings appSettings)
        {
            if (Environment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                  );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Delete database, add again and seed
            if(appSettings.ResetDatabase)
            { 
                hubContext.Database.EnsureDeleted();
                hubContext.Database.EnsureCreated();
                SeedData.SeedDatabase(Environment, hubContext);
            }

        }
    }
}
