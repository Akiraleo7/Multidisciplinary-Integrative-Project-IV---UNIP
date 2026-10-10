using HEJCareSuite.Application.Patients;
using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Tests;

public sealed class RegisterPatientTests
{
    [Fact]
    public async Task ExecuteAsync_ValidCommand_PersistsAndReturnsPatient()
    {
        // Arrange
        var repository = new InMemoryPatientRepository();
        var useCase = new RegisterPatient(repository);
        var command = new RegisterPatientCommand(
            "Maria Silva",
            new DateOnly(1990, 5, 20),
            "12345678900");

        // Act
        var patient = await useCase.ExecuteAsync(command, CancellationToken.None);

        // Assert
        var storedPatients = await repository.ListAsync(CancellationToken.None);
        var storedPatient = Assert.Single(storedPatients);
        Assert.Equal(patient.Id, storedPatient.Id);
        Assert.Equal(command.FullName, storedPatient.FullName);
        Assert.Equal(command.DocumentNumber, storedPatient.DocumentNumber);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCommand_DoesNotPersistPatient()
    {
        // Arrange
        var repository = new InMemoryPatientRepository();
        var useCase = new RegisterPatient(repository);
        var command = new RegisterPatientCommand(
            "",
            new DateOnly(1990, 5, 20),
            "12345678900");

        // Act
        var action = () => useCase.ExecuteAsync(command, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);
        Assert.Empty(await repository.ListAsync(CancellationToken.None));
    }

    private sealed class InMemoryPatientRepository : IPatientRepository
    {
        private readonly List<Patient> patients = [];

        public Task AddAsync(Patient patient, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            patients.Add(patient);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Patient>> ListAsync(
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<IReadOnlyCollection<Patient>>(patients);
        }
    }
}
