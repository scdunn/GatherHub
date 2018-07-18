using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Data;
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
            //Setup session configuration
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
            });


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
                        options.LoginPath = new PathString("/auth/signin");
                        options.AccessDeniedPath = new PathString("/auth/denied");
                        options.Cookie.Name = "ClientCookieAuth";
                    });

          
            services.AddDbContext<HubContext>(options =>
                 options.UseSqlite("Filename=./hub.db"));

            services.AddDbContext<ActivityContext>(options =>
                 options.UseSqlite("Filename=./hub.db"));

            //load typed appsettings as singleton service
            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILogger, ActivityLogger>();
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
