using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Api.Contracts.Triage;

public sealed record TriageResponse(
    Guid Id,
    Guid PatientId,
    TriageRiskLevel RiskLevel,
    string ChiefComplaint,
    DateTime RecordedAtUtc);
