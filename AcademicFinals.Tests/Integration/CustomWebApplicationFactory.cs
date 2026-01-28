using System.Linq;
using Microsoft.AspNetCore.Authentication;
using AcademicFinals.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicFinals.Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));

                if (descriptor != null)
                    services.Remove(descriptor);


                var dbConfigDescriptor = services.SingleOrDefault(d =>
                     d.ServiceType.Name == "IDbContextOptionsConfiguration`1" &&
                     d.ServiceType.GenericTypeArguments.Contains(typeof(ApplicationContext)));

                if (dbConfigDescriptor != null)
                {
                    services.Remove(dbConfigDescriptor);
                }

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            });


        }
    }
}