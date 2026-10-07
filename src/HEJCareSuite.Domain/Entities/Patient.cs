namespace HEJCareSuite.Domain.Entities;

public sealed class Patient
{
    private Patient()
    {
    }

    public Patient(string fullName, DateOnly birthDate, string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Patient full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Patient document number is required.", nameof(documentNumber));

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        BirthDate = birthDate;
        DocumentNumber = documentNumber.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
}
