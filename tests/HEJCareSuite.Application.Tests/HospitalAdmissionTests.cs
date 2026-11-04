using HEJCareSuite.Application.Hospitalization;
using HEJCareSuite.Domain.Entities;
using Xunit;

namespace HEJCareSuite.Application.Tests;

public sealed class HospitalAdmissionTests
{
    [Fact]
    public async Task AdmitAsync_ValidCommand_PersistsAdmission()
    {
        var repository = new FakeHospitalAdmissionRepository();
        var useCase = new AdmitPatient(repository);

        var admission = await useCase.ExecuteAsync(
            new AdmitPatientCommand(Guid.NewGuid(), "A-101"),
            CancellationToken.None);

        Assert.Same(admission, Assert.Single(repository.Items));
    }

    private sealed class FakeHospitalAdmissionRepository : IHospitalAdmissionRepository
    {
        public List<HospitalAdmission> Items { get; } = [];

        public Task AddAsync(
            HospitalAdmission admission,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Items.Add(admission);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<HospitalAdmission>> ListByPatientAsync(
            Guid patientId,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyCollection<HospitalAdmission>>(
                Items.Where(item => item.PatientId == patientId).ToArray());

        public Task<bool> DischargeAsync(
            Guid admissionId,
            Guid patientId,
            string dischargeNotes,
            DateTime dischargedAtUtc,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var admission = Items.SingleOrDefault(item => item.Id == admissionId);
            if (admission is null)
                return Task.FromResult(false);

            admission.Discharge(dischargeNotes, dischargedAtUtc);
            return Task.FromResult(true);
        }
    }
}
