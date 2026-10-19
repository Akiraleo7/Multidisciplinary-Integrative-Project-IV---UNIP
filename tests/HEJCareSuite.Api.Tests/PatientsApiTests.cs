using System.Net;
using System.Net.Http.Json;
using HEJCareSuite.Api.Contracts.Patients;
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
        var patient = await response.Content.ReadFromJsonAsync<PatientResponse>();
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
        var patients = await response.Content.ReadFromJsonAsync<PatientResponse[]>();
        Assert.Contains(
            patients!,
            patient => patient.DocumentNumber == expectedPatient.DocumentNumber
                && patient.FullName == expectedPatient.FullName
                && patient.BirthDate == expectedPatient.BirthDate);
    }

    [Fact]
    public async Task PostPatients_InvalidName_ReturnsProblemDetails()
    {
        // Arrange
        using var factory = new PatientsApiFactory();
        using var client = factory.CreateClient();
        var request = new RegisterPatientRequest(
            " ",
            new DateOnly(1990, 5, 20),
            Guid.NewGuid().ToString("N"));

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/patients", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        Assert.NotNull(problem);
        Assert.Equal("Invalid patient data.", problem.Title);
    }

    [Fact]
    public async Task PutPatients_ExistingPatient_UpdatesPatient()
    {
        using var factory = new PatientsApiFactory();
        using var client = factory.CreateClient();
        var patient = new Patient("Maria Silva", new DateOnly(1990, 5, 20), "old-document");
        factory.Repository.Add(patient);

        var request = new RegisterPatientRequest(
            "Maria Santos",
            new DateOnly(1991, 6, 21),
            "new-document");

        var response = await client.PutAsJsonAsync($"/api/v1/patients/{patient.Id}", request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var updated = Assert.Single(await factory.Repository.ListAsync(CancellationToken.None));
        Assert.Equal(request.FullName, updated.FullName);
        Assert.Equal(request.BirthDate, updated.BirthDate);
        Assert.Equal(request.DocumentNumber, updated.DocumentNumber);
    }

    [Fact]
    public async Task DeletePatients_ExistingPatient_RemovesPatient()
    {
        using var factory = new PatientsApiFactory();
        using var client = factory.CreateClient();
        var patient = new Patient("Maria Silva", new DateOnly(1990, 5, 20), "delete-document");
        factory.Repository.Add(patient);

        var response = await client.DeleteAsync($"/api/v1/patients/{patient.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Empty(await factory.Repository.ListAsync(CancellationToken.None));
    }
}

public sealed record ProblemDetailsResponse(string? Title, string? Detail);

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

    public Task<bool> UpdateAsync(
        Guid patientId,
        string fullName,
        DateOnly birthDate,
        string documentNumber,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (syncRoot)
        {
            var patient = patients.SingleOrDefault(candidate => candidate.Id == patientId);
            if (patient is null)
                return Task.FromResult(false);

            patient.UpdateDetails(fullName, birthDate, documentNumber);
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(Guid patientId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (syncRoot)
        {
            return Task.FromResult(patients.RemoveAll(patient => patient.Id == patientId) > 0);
        }
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
