using HEJCareSuite.Api.Contracts.Hospitalization;
using HEJCareSuite.Application.Hospitalization;
using HEJCareSuite.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/patients/{patientId:guid}/admissions")]
public sealed class HospitalizationController(
    AdmitPatient admitPatient,
    DischargePatient dischargePatient,
    IHospitalAdmissionRepository repository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        Guid patientId,
        CancellationToken cancellationToken)
    {
        var admissions = await repository.ListByPatientAsync(patientId, cancellationToken);
        return Ok(admissions.Select(ToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Admit(
        Guid patientId,
        AdmitPatientRequest request,
        CancellationToken cancellationToken)
    {
        var admission = await admitPatient.ExecuteAsync(
            new AdmitPatientCommand(patientId, request.BedNumber),
            cancellationToken);
        return CreatedAtAction(nameof(List), new { patientId }, ToResponse(admission));
    }

    [HttpPost("{admissionId:guid}/discharge")]
    public async Task<IActionResult> Discharge(
        Guid patientId,
        Guid admissionId,
        DischargePatientRequest request,
        CancellationToken cancellationToken)
    {
        var discharged = await dischargePatient.ExecuteAsync(
            new DischargePatientCommand(admissionId, patientId, request.DischargeNotes),
            cancellationToken);
        return discharged ? NoContent() : NotFound();
    }

    private static HospitalAdmissionResponse ToResponse(HospitalAdmission admission) =>
        new(
            admission.Id,
            admission.PatientId,
            admission.BedNumber,
            admission.Status,
            admission.AdmittedAtUtc,
            admission.DischargedAtUtc,
            admission.DischargeNotes);
}
