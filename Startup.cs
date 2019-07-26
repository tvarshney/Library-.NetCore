using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Features;
using System.Net;


namespace MvcMovie
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            
            Configuration = configuration;
           
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
        //    services.AddIdentity<User, Role>().AddEntityFrameworkStores<DbContext>()
        //         .AddDefaultTokenProviders();

        //    services.AddTransient<IRoleStore<Role>,RoleStore>();
        //    services.AddTransient<IUserStore<User>, UserStore>();


            services.AddDbContext<DataContext>(options =>
            //options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DataContext>(options =>
            //options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            options.UseMySql(Configuration.GetConnectionString("MoodleConnection")));
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                   
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                });
            // services.AddAuthorization(options =>
            //     {
            //         options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
            //     });

            services.AddMvc();
            services.Configure<FormOptions>(x => {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
        });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //env.EnvironmentName = EnvironmentName.Production;
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                 app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
                
               app.UseStatusCodePages("text/plain", "Error. Status code : {0}");

                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = context =>
                    {
                        context.Context.Response.Headers.Add("Cache-Control", "no-cache");
                    }
                });

                

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{page?}");

                     routes.MapRoute(
                        name: "Search", 
                        template: "{controller=Home}/{action=Index}/{searchQuery?}/{page?}");

                        routes.MapRoute(
                        name: "POST",
                        template: "{controller=gcdnuser}/{action=redirect}/{user?}");

                        routes.MapRoute(
                        name: "Api/auth",
                        template: "{controller=gcdnuser}/{action=redirectTextJson}/");

                        routes.MapRoute(
                        name: "Api/logout",
                        template: "{controller=gcdnuser}/{action=logout}/");


                //  routes.MapRoute(
                //      name: "admin",
                //      template: "{controller}/{action}/{id?}");
                      
            });

        
                        // app.Use(async (context, next) =>
                        // {
                        //     if (!context.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Files/"))
                        //     {
                        //         context.Response.Redirect("/Account/Login");
                        //         return;
                        //     }
                        //     await next.Invoke();
                        // });   
        }
    }
}
