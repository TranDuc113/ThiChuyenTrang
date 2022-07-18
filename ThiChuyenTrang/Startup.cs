using DemoJwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThiChuyenTrang.Models;
using ThiChuyenTrang.Models.EF;

namespace ThiChuyenTrang
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
            services.AddDbContext<ThiChuyenTrangContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddSession();
            services.AddControllersWithViews();
            var tokenKey = Configuration.GetValue<string>("TokenKey"); 
            services.ConfigureApplicationCookie(options => options.LoginPath = "/login");
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton<IJwtAuthenicationManager>(new JwtAuthenicationManager(tokenKey));
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>(); 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                if (!string.IsNullOrEmpty(context.Session.GetString("UserAdminToken")))
                {
                    UserInfoModel userInfoModel = JsonSerializer.Deserialize<UserInfoModel>(context.Session.GetString("UserAdminToken")); ;
                    if (!string.IsNullOrEmpty(userInfoModel.Token))
                    {
                        context.Request.Headers.Add("Authorization", "Bearer " + userInfoModel.Token);
                    }
                    
                }
                await next();
            });
            app.UseRouting();
            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == 404|| response.StatusCode == 500)
                    response.Redirect("/Admin/login/index");
                return Task.CompletedTask;
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                "Admin",
                "Admin",
                "Admin/{controller=Login}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
