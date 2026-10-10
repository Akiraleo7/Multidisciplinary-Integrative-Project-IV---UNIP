using HEJCareSuite.Application.Patients;
using HEJCareSuite.Infrastructure.Patients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var sqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? throw new InvalidOperationException(
        "The SqlServer connection string is not configured.");

builder.Services.AddSingleton<IPatientRepository>(
    new SqlPatientRepository(sqlServerConnectionString));
builder.Services.AddScoped<RegisterPatient>();

var app = builder.Build();

app.MapControllers();
app.Run();

public partial class Program;
