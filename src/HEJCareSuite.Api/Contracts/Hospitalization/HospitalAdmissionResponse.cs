using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Api.Contracts.Hospitalization;

public sealed record HospitalAdmissionResponse(
    Guid Id,
    Guid PatientId,
    string BedNumber,
    AdmissionStatus Status,
    DateTime AdmittedAtUtc,
    DateTime? DischargedAtUtc,
    string? DischargeNotes);
