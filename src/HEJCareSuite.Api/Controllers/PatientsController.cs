using HEJCareSuite.Application.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HEJCareSuite.Api.Controllers;

[ApiController]
[Route("api/v1/patients")]
public sealed class PatientsController(RegisterPatient registerPatient, IPatientRepository patientRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var patients = await patientRepository.ListAsync(cancellationToken);
        return Ok(patients);
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        RegisterPatientCommand command,
        CancellationToken cancellationToken)
    {
        var patient = await registerPatient.ExecuteAsync(command, cancellationToken);
        return CreatedAtAction(nameof(List), new { id = patient.Id }, patient);
    }
}
