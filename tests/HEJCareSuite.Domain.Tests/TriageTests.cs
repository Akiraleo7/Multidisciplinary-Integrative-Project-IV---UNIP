using HEJCareSuite.Domain.Entities;
using Xunit;

namespace HEJCareSuite.Domain.Tests;

public sealed class TriageTests
{
    [Fact]
    public void Constructor_ValidData_CreatesTriage()
    {
        var patientId = Guid.NewGuid();

        var triage = new Triage(
            patientId,
            TriageRiskLevel.Red,
            "Chest pain",
            DateTime.UtcNow);

        Assert.Equal(patientId, triage.PatientId);
        Assert.Equal(TriageRiskLevel.Red, triage.RiskLevel);
        Assert.Equal("Chest pain", triage.ChiefComplaint);
    }

    [Fact]
    public void Constructor_EmptyComplaint_Throws()
    {
        var action = () => new Triage(
            Guid.NewGuid(),
            TriageRiskLevel.Green,
            " ",
            DateTime.UtcNow);

        Assert.Throws<ArgumentException>(action);
    }
}
