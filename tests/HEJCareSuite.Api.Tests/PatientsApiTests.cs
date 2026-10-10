using System.Net;
using System.Net.Http.Json;
using HEJCareSuite.Application.Patients;
using HEJCareSuite.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace HEJCareSuite.Api.Tests;

public sealed class PatientsApiTests
{
    [Fact]
    public async Task PostPatients_ValidRequest_ReturnsCreatedPatient()
    {
        // Arrange
        using var factory = new PatientsApiFactory();
        using var client = factory.CreateClient();
        factory.Repository.Clear();
        var request = new RegisterPatientCommand(
            "Maria Silva",
            new DateOnly(1990, 5, 20),
            Guid.NewGuid().ToString("N"));

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/patients", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var patient = await response.Content.ReadFromJsonAsync<Patient>();
        Assert.NotNull(patient);
        Assert.Equal(request.FullName, patient.FullName);
        Assert.Equal(request.BirthDate, patient.BirthDate);
        Assert.Equal(request.DocumentNumber, patient.DocumentNumber);
    }

    [Fact]
    public async Task GetPatients_RepositoryContainsPatient_ReturnsOkWithPatient()
    {
        // Arrange
        using var factory = new PatientsApiFactory();
        using var client = factory.CreateClient();
        factory.Repository.Clear();
        var expectedPatient = new Patient(
            "Joao Souza",
            new DateOnly(1985, 10, 12),
            Guid.NewGuid().ToString("N"));
        factory.Repository.Add(expectedPatient);

        // Act
        var response = await client.GetAsync("/api/v1/patients");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var patients = await response.Content.ReadFromJsonAsync<Patient[]>();
        Assert.Contains(patients!, patient => patient.Id == expectedPatient.Id);
    }
}

public sealed class PatientsApiFactory : WebApplicationFactory<Program>
{
    internal InMemoryPatientRepository Repository { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SqlServer"] =
                    "Server=localhost;Database=HEJCareSuite;Integrated Security=True"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IPatientRepository>();
            services.AddSingleton<IPatientRepository>(Repository);
        });
    }
}

public sealed class InMemoryPatientRepository : IPatientRepository
{
    private readonly List<Patient> patients = [];
    private readonly object syncRoot = new();

    public void Add(Patient patient)
    {
        lock (syncRoot)
        {
            patients.Add(patient);
        }
    }

    public void Clear()
    {
        lock (syncRoot)
        {
            patients.Clear();
        }
    }

    public Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Add(patient);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Patient>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (syncRoot)
        {
            return Task.FromResult<IReadOnlyCollection<Patient>>(patients.ToArray());
        }
    }
}
