using HEJCareSuite.Application.Patients;
using HEJCareSuite.Application.Authentication;
using HEJCareSuite.Api.Errors;
using HEJCareSuite.Infrastructure.Authentication;
using HEJCareSuite.Infrastructure.Patients;
using HEJCareSuite.Application.Triage;
using HEJCareSuite.Infrastructure.Triage;
using HEJCareSuite.Application.Hospitalization;
using HEJCareSuite.Infrastructure.Hospitalization;
using HEJCareSuite.Application.Mobile;
using HEJCareSuite.Infrastructure.Mobile;
using HEJCareSuite.Application.Accessibility;
using HEJCareSuite.Infrastructure.Accessibility;
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
builder.Services.AddScoped<AdmitPatient>();
builder.Services.AddScoped<DischargePatient>();
builder.Services.AddSingleton<IHospitalAdmissionRepository>(
    new SqlHospitalAdmissionRepository(sqlServerConnectionString));
builder.Services.AddScoped<AuthenticateUser>();
builder.Services.AddSingleton<IUserRepository>(
    new SqlUserRepository(sqlServerConnectionString));
builder.Services.AddSingleton<IPasswordVerifier, Pbkdf2PasswordVerifier>();

// Mobile and Accessibility
builder.Services.AddSingleton<IStaffTaskRepository>(
    new SqlStaffTaskRepository(sqlServerConnectionString));
builder.Services.AddSingleton<ICriticalAlertRepository>(
    new SqlCriticalAlertRepository(sqlServerConnectionString));
builder.Services.AddSingleton<IVLibrasService>(
    new SqlVlibrasRepository(sqlServerConnectionString));
builder.Services.AddScoped<CreateStaffTask>();
builder.Services.AddScoped<CompleteStaffTask>();
builder.Services.AddScoped<CreateCriticalAlert>();
builder.Services.AddScoped<AcknowledgeCriticalAlert>();
builder.Services.AddScoped<TranslateText>();


var app = builder.Build();

app.UseExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "HEJCareSuite.Api"
}));

// Register mobile and accessibility endpoints
var mobileGroup = app.MapGroup("/api/v1/mobile").WithTags("Mobile").RequireAuthorization();

mobileGroup.MapPost("/tasks", async (CreateStaffTask handler, CreateStaffTaskCommand request, CancellationToken ct) =>
    Results.Ok(await handler.ExecuteAsync(request, ct)));

mobileGroup.MapPost("/tasks/{taskId:guid}/complete", async (CompleteStaffTask handler, Guid taskId, Guid patientId, CancellationToken ct) =>
    Results.Ok(await handler.ExecuteAsync(new CompleteStaffTaskCommand(taskId, patientId), ct)));

mobileGroup.MapGet("/tasks/patient/{patientId:guid}/pending", async (IStaffTaskRepository repo, Guid patientId, CancellationToken ct) =>
    Results.Ok(await repo.ListByPatientAsync(patientId, true, ct)));

mobileGroup.MapPost("/alerts", async (CreateCriticalAlert handler, CreateCriticalAlertCommand request, CancellationToken ct) =>
    Results.Ok(await handler.ExecuteAsync(request, ct)));

mobileGroup.MapGet("/alerts/unacknowledged/{patientId?}", async (ICriticalAlertRepository repo, Guid? patientId, CancellationToken ct) =>
    Results.Ok(await repo.ListUnacknowledgedAsync(patientId, ct)));

mobileGroup.MapPost("/alerts/{alertId:guid}/acknowledge", async (AcknowledgeCriticalAlert handler, Guid alertId, Guid? userId, CancellationToken ct) =>
    Results.Ok(await handler.ExecuteAsync(new AcknowledgeCriticalAlertCommand(alertId, userId), ct)));

var accessibilityGroup = app.MapGroup("/api/v1/accessibility").WithTags("Accessibility");

accessibilityGroup.MapPost("/translate", async (TranslateText handler, TranslateTextCommand request, CancellationToken ct) =>
    Results.Ok(await handler.ExecuteAsync(request, ct)));

app.MapControllers();
app.Run();


public partial class Program;
