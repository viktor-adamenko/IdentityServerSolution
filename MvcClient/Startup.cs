using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcClient.Services;

namespace MvcClient
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookie";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie", config => { config.AccessDeniedPath = "/Error/AccessDenied"; })
                .AddOpenIdConnect("oidc", config =>
                {
                    config.SignedOutCallbackPath = "/Home";

                    config.Authority = Configuration["IdentityServerSettings:AuthorityUrl"];
                    config.ClientId = Configuration["IdentityServerSettings:ClientId"];
                    config.ClientSecret = Configuration["IdentityServerSettings:ClientSecret"];

                    config.ResponseType = "code";
                    //config.GetClaimsFromUserInfoEndpoint = true;

                    config.Scope.Add(Configuration["IdentityServerSettings:Scopes:0"]);
                    config.Scope.Add(Configuration["IdentityServerSettings:Scopes:1"]);
                    config.SaveTokens = true;
                });

            // services.AddAuthorization(config =>
            // {
            //     config.AddPolicy("ForManagers", policy =>
            //     {
            //         policy.RequireRole("Admin", "Manager");
            //
            //         policy.RequireClaim("department", new List<string> { "ManagerDepartment", "AdminDepartment" });
            //     });
            // });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.Configure<IdentityServerSettings>(Configuration.GetSection("ApiIdentityServerSettings"));
            services.AddSingleton<ITokenService, TokenService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                //app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}