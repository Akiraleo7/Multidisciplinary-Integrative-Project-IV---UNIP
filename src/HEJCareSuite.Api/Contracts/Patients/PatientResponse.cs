namespace HEJCareSuite.Api.Contracts.Patients;

public sealed record PatientResponse(
    Guid Id,
    string FullName,
    DateOnly BirthDate,
    string DocumentNumber,
    DateTime CreatedAtUtc);
