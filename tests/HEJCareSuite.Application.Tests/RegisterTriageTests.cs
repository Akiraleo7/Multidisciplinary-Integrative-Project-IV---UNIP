using HEJCareSuite.Application.Triage;
using HEJCareSuite.Domain.Entities;
using Xunit;

namespace HEJCareSuite.Application.Tests;

public sealed class RegisterTriageTests
{
    [Fact]
    public async Task ExecuteAsync_ValidCommand_PersistsTriage()
    {
        var repository = new FakeTriageRepository();
        var useCase = new RegisterTriage(repository);

        var triage = await useCase.ExecuteAsync(
            new RegisterTriageCommand(
                Guid.NewGuid(),
                TriageRiskLevel.Yellow,
                "Fever"),
            CancellationToken.None);

        Assert.Same(triage, Assert.Single(repository.Items));
    }

    private sealed class FakeTriageRepository : ITriageRepository
    {
        public List<global::HEJCareSuite.Domain.Entities.Triage> Items { get; } = [];

        public Task AddAsync(
            global::HEJCareSuite.Domain.Entities.Triage triage,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Items.Add(triage);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<global::HEJCareSuite.Domain.Entities.Triage>> ListByPatientAsync(
            Guid patientId,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyCollection<global::HEJCareSuite.Domain.Entities.Triage>>(
                Items.Where(item => item.PatientId == patientId).ToArray());
    }
}
