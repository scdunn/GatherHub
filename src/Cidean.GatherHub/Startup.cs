using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Data;
using Cidean.GatherHub.Core.Helpers;
using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cidean.GatherHub
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

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

            services.AddDbContext<HubContext>(options =>
                 options.UseSqlite("Filename=./Data/hub.db"));

            services.AddDbContext<ActivityContext>(options =>
                 options.UseSqlite("Filename=./Data/hub.db"));

            //load typed appsettings as singleton service
            services.AddSingleton(this.Configuration.GetSection("AppSettings").Get<AppSettings>());

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IActivityLogger, ActivityLogger>();
            services.AddTransient<Mailer, Mailer>();

            var appSettings = services.BuildServiceProvider().GetService<AppSettings>();

            //Configure Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(appSettings.SessionTimeout);
                options.Cookie.HttpOnly = true;
                
            });

            //Add cookie authentication configuration
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie("admin",
                    options =>
                    {
                        options.LoginPath = new PathString("/admin/auth/signin");
                        options.AccessDeniedPath = new PathString("/admin/auth/denied");
                        options.Cookie.Name = "AdminCookieAuth";

                    })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/account/signin");
                        options.AccessDeniedPath = new PathString("/account/denied");
                        options.Cookie.Name = "ClientCookieAuth";
                    });

          
            
            services.AddMvc();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,  HubContext hubContext, AppSettings appSettings)
        {
            app.UseAuthentication();

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
            app.UseSession();
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
