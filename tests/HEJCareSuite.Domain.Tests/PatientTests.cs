using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Domain.Tests;

public sealed class PatientTests
{
    [Fact]
    public void Constructor_ValidData_TrimsTextAndCreatesIdentity()
    {
        // Arrange
        var birthDate = new DateOnly(1990, 5, 20);

        // Act
        var patient = new Patient("  Maria Silva  ", birthDate, " 12345678900 ");

        // Assert
        Assert.NotEqual(Guid.Empty, patient.Id);
        Assert.Equal("Maria Silva", patient.FullName);
        Assert.Equal("12345678900", patient.DocumentNumber);
        Assert.Equal(birthDate, patient.BirthDate);
        Assert.NotEqual(default, patient.CreatedAtUtc);
    }

    [Fact]
    public void Constructor_BlankFullName_ThrowsArgumentException()
    {
        // Arrange
        var birthDate = new DateOnly(1990, 5, 20);

        // Act
        var action = () => new Patient(" ", birthDate, "12345678900");

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_BlankDocumentNumber_ThrowsArgumentException()
    {
        // Arrange
        var birthDate = new DateOnly(1990, 5, 20);

        // Act
        var action = () => new Patient("Maria Silva", birthDate, " ");

        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}
