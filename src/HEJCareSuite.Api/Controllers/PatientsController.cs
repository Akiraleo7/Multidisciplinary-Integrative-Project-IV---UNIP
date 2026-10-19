using HEJCareSuite.Application.Patients;
using HEJCareSuite.Api.Contracts.Patients;
using HEJCareSuite.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Controllers;

[ApiController]
[Route("api/v1/patients")]
public sealed class PatientsController(
    RegisterPatient registerPatient,
    UpdatePatient updatePatient,
    DeletePatient deletePatient,
    IPatientRepository patientRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var patients = await patientRepository.ListAsync(cancellationToken);
        return Ok(patients.Select(ToResponse));
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        RegisterPatientRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterPatientCommand(
            request.FullName,
            request.BirthDate,
            request.DocumentNumber);
        var patient = await registerPatient.ExecuteAsync(command, cancellationToken);
        return CreatedAtAction(nameof(List), new { id = patient.Id }, ToResponse(patient));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        RegisterPatientRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await updatePatient.ExecuteAsync(
            new UpdatePatientCommand(id, request.FullName, request.BirthDate, request.DocumentNumber),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await deletePatient.ExecuteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private static PatientResponse ToResponse(Patient patient) =>
        new(patient.Id, patient.FullName, patient.BirthDate, patient.DocumentNumber, patient.CreatedAtUtc);
}
