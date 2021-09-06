using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InitializeDatabase(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void InitializeDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                //var userManager = scope.ServiceProvider
                //    .GetRequiredService<UserManager<IdentityUser>>();

                //var user = new IdentityUser("bob");
                //userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                //userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", "Admin"));

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var adminRole = new IdentityRole("Admin");
                var managerRole = new IdentityRole("Manager");
                var userRole = new IdentityRole("User");

                roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                roleManager.CreateAsync(managerRole).GetAwaiter().GetResult();
                roleManager.CreateAsync(userRole).GetAwaiter().GetResult();

                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Configuration.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Configuration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Configuration.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Configuration.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
