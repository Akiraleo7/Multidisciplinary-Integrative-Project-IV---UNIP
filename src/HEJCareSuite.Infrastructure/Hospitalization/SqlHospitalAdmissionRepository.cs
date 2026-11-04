using HEJCareSuite.Application.Hospitalization;
using HEJCareSuite.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Hospitalization;

public sealed class SqlHospitalAdmissionRepository(string connectionString)
    : IHospitalAdmissionRepository
{
    public async Task AddAsync(
        HospitalAdmission admission,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.HospitalAdmission
                (AdmissionId, PatientId, BedNumber, Status, AdmittedAtUtc)
            VALUES
                (@AdmissionId, @PatientId, @BedNumber, @Status, @AdmittedAtUtc);
            """;
        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@AdmissionId", admission.Id);
        command.Parameters.AddWithValue("@PatientId", admission.PatientId);
        command.Parameters.AddWithValue("@BedNumber", admission.BedNumber);
        command.Parameters.AddWithValue("@Status", (int)admission.Status);
        command.Parameters.AddWithValue("@AdmittedAtUtc", admission.AdmittedAtUtc);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<HospitalAdmission>> ListByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            SELECT AdmissionId, PatientId, BedNumber, Status,
                   AdmittedAtUtc, DischargedAtUtc, DischargeNotes
            FROM dbo.HospitalAdmission
            WHERE PatientId = @PatientId
            ORDER BY AdmittedAtUtc DESC;
            """;
        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@PatientId", patientId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var admissions = new List<HospitalAdmission>();
        while (await reader.ReadAsync(cancellationToken))
        {
            admissions.Add(HospitalAdmission.Rehydrate(
                reader.GetGuid(0),
                reader.GetGuid(1),
                reader.GetString(2),
                (AdmissionStatus)reader.GetInt32(3),
                reader.GetDateTime(4),
                reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                reader.IsDBNull(6) ? null : reader.GetString(6)));
        }

        return admissions;
    }

    public async Task<bool> DischargeAsync(
        Guid admissionId,
        Guid patientId,
        string dischargeNotes,
        DateTime dischargedAtUtc,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            UPDATE dbo.HospitalAdmission
            SET Status = @Status,
                DischargedAtUtc = @DischargedAtUtc,
                DischargeNotes = @DischargeNotes
            WHERE AdmissionId = @AdmissionId
              AND PatientId = @PatientId
              AND Status = @ActiveStatus;
            """;
        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Status", (int)AdmissionStatus.Discharged);
        command.Parameters.AddWithValue("@DischargedAtUtc", dischargedAtUtc);
        command.Parameters.AddWithValue("@DischargeNotes", dischargeNotes);
        command.Parameters.AddWithValue("@AdmissionId", admissionId);
        command.Parameters.AddWithValue("@PatientId", patientId);
        command.Parameters.AddWithValue("@ActiveStatus", (int)AdmissionStatus.Active);
        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }
}
