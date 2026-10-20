using HEJCareSuite.Application.Patients;
using HEJCareSuite.Application.Authentication;
using HEJCareSuite.Api.Errors;
using HEJCareSuite.Infrastructure.Authentication;
using HEJCareSuite.Infrastructure.Patients;
using HEJCareSuite.Application.Triage;
using HEJCareSuite.Infrastructure.Triage;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "hejcare.auth";
        options.LoginPath = "/api/v1/auth/login";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<PatientExceptionHandler>();
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException(
        "The SqlServer connection string is not configured.");

builder.Services.AddSingleton<IPatientRepository>(
    new SqlPatientRepository(sqlServerConnectionString));
builder.Services.AddScoped<RegisterPatient>();
builder.Services.AddScoped<UpdatePatient>();
builder.Services.AddScoped<DeletePatient>();
builder.Services.AddScoped<RegisterTriage>();
builder.Services.AddSingleton<ITriageRepository>(
    new SqlTriageRepository(sqlServerConnectionString));
builder.Services.AddScoped<AuthenticateUser>();
builder.Services.AddSingleton<IUserRepository>(
    new SqlUserRepository(sqlServerConnectionString));
builder.Services.AddSingleton<IPasswordVerifier, Pbkdf2PasswordVerifier>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program;
