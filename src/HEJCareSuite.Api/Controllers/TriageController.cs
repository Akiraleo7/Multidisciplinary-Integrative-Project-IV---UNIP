using HEJCareSuite.Api.Contracts.Triage;
using HEJCareSuite.Application.Triage;
using HEJCareSuite.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/patients/{patientId:guid}/triage")]
public sealed class TriageController(RegisterTriage registerTriage, ITriageRepository triageRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(Guid patientId, CancellationToken cancellationToken)
    {
        var triages = await triageRepository.ListByPatientAsync(patientId, cancellationToken);
        return Ok(triages.Select(ToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        Guid patientId,
        RegisterTriageRequest request,
        CancellationToken cancellationToken)
    {
        var triage = await registerTriage.ExecuteAsync(
            new RegisterTriageCommand(patientId, request.RiskLevel, request.ChiefComplaint),
            cancellationToken);
        return CreatedAtAction(nameof(List), new { patientId }, ToResponse(triage));
    }

    private static TriageResponse ToResponse(Triage triage) =>
        new(triage.Id, triage.PatientId, triage.RiskLevel, triage.ChiefComplaint, triage.RecordedAtUtc);
}
