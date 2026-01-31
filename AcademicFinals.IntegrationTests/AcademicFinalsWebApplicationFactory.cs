using AcademicFinals.API.Models;
using AcademicFinals.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

namespace AcademicFinals.IntegrationTests
{
    public class AcademicFinalsWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove SQL Server configuration
                services.RemoveAll(typeof(DbContextOptions<ApplicationContext>));
                services.RemoveAll(typeof(DbContextOptions));
                services.RemoveAll(typeof(DbConnection));

                // Override DB options
                services.AddScoped<DbContextOptions<ApplicationContext>>(sp =>
                {
                    return new DbContextOptionsBuilder<ApplicationContext>()
                        .UseInMemoryDatabase("TestingDB")
                        .UseApplicationServiceProvider(sp)
                        .Options;
                });

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        }
    }
}