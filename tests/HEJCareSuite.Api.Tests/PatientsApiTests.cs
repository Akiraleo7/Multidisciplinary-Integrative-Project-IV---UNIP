using System.Net;
using System.Net.Http.Json;
using HEJCareSuite.Application.Patients;
using HEJCareSuite.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HEJCareSuite.Api.Tests;

public sealed class PatientsApiTests : IClassFixture<PatientsApiFactory>
{
    private readonly PatientsApiFactory factory;
    private readonly HttpClient client;

    public PatientsApiTests(PatientsApiFactory factory)
    {
        this.factory = factory;
        client = factory.CreateClient();
    }

    [Fact]
    public async Task PostPatients_ValidRequest_ReturnsCreatedPatient()
    {
        // Arrange
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
        var patient = Assert.Single(patients!);
        Assert.Equal(expectedPatient.Id, patient.Id);
    }
}

public sealed class PatientsApiFactory : WebApplicationFactory<Program>
{
    public InMemoryPatientRepository Repository { get; } = new();

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

internal sealed class InMemoryPatientRepository : IPatientRepository
{
    private readonly List<Patient> patients = [];

    public void Add(Patient patient) => patients.Add(patient);

    public void Clear() => patients.Clear();

    public Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        patients.Add(patient);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Patient>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyCollection<Patient>>(patients.ToArray());
    }
}
