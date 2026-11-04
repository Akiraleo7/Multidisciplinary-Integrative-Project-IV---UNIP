using HEJCareSuite.Domain.Entities;
using Xunit;

namespace HEJCareSuite.Domain.Tests;

public sealed class HospitalAdmissionTests
{
    [Fact]
    public void Constructor_ValidData_CreatesActiveAdmission()
    {
        var patientId = Guid.NewGuid();

        var admission = new HospitalAdmission(patientId, "A-101", DateTime.UtcNow);

        Assert.Equal(patientId, admission.PatientId);
        Assert.Equal("A-101", admission.BedNumber);
        Assert.Equal(AdmissionStatus.Active, admission.Status);
        Assert.Null(admission.DischargedAtUtc);
    }

    [Fact]
    public void Constructor_EmptyBed_Throws()
    {
        var action = () => new HospitalAdmission(
            Guid.NewGuid(),
            " ",
            DateTime.UtcNow);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Discharge_ActiveAdmission_SetsDischargeData()
    {
        var admission = new HospitalAdmission(Guid.NewGuid(), "A-101", DateTime.UtcNow);
        var dischargedAt = DateTime.UtcNow.AddHours(4);

        admission.Discharge("Stable condition", dischargedAt);

        Assert.Equal(AdmissionStatus.Discharged, admission.Status);
        Assert.Equal("Stable condition", admission.DischargeNotes);
        Assert.Equal(dischargedAt, admission.DischargedAtUtc);
    }
}
