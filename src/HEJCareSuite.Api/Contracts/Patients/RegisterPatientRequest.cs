namespace HEJCareSuite.Api.Contracts.Patients;

public sealed record RegisterPatientRequest(
    string FullName,
    DateOnly BirthDate,
    string DocumentNumber);
