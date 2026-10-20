using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Api.Contracts.Triage;

public sealed record RegisterTriageRequest(
    TriageRiskLevel RiskLevel,
    string ChiefComplaint);
