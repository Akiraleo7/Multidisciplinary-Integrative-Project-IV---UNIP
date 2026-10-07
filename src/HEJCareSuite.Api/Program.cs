using HEJCareSuite.Application.Patients;
using HEJCareSuite.Infrastructure.Patients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IPatientRepository, InMemoryPatientRepository>();
builder.Services.AddScoped<RegisterPatient>();

var app = builder.Build();

app.MapControllers();
app.Run();
