using AcademicFinals.API.DTOs;
using AcademicFinals.API.Extensions;
using AcademicFinals.API.Mappers;
using AcademicFinals.API.Models;
using AcademicFinals.API.Repository;
using AcademicFinals.API.Services;
using AcademicFinals.API.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSwaggerServices();

builder.Services.AddCorsServices(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidator>();

builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IValidator<SubjectPostDto>, SubjectPostValidator>();
builder.Services.AddScoped<IValidator<SubjectPutDto>, SubjectPutValidator>();
builder.Services.AddScoped<IExamDateService, ExamDateService>();
builder.Services.AddScoped<IValidator<ExamDatePostDto>, ExamDatePostValidator>();
builder.Services.AddScoped<IValidator<ExamDatePutDto>, ExamDatePutValidator>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@localhost.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdmin, "@Admin123");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al realizar el seeding de datos.");
    }
}

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

    await next.Invoke();

    logger.LogInformation($"Response: {context.Response.StatusCode}");
});

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", options =>
{
    options.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.MapControllers();
app.Run();

public partial class Program { }