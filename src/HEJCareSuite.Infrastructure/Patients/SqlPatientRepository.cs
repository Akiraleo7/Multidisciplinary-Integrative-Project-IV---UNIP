using HEJCareSuite.Application.Patients;
using HEJCareSuite.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Patients;

public sealed class SqlPatientRepository(string connectionString) : IPatientRepository
{
    public async Task AddAsync(Patient patient, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.Patient
                (PatientId, FullName, BirthDate, DocumentNumber, CreatedAtUtc)
            VALUES
                (@PatientId, @FullName, @BirthDate, @DocumentNumber, @CreatedAtUtc);
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@PatientId", patient.Id);
        command.Parameters.AddWithValue("@FullName", patient.FullName);
        command.Parameters.AddWithValue("@BirthDate", patient.BirthDate.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@DocumentNumber", patient.DocumentNumber);
        command.Parameters.AddWithValue("@CreatedAtUtc", patient.CreatedAtUtc);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Patient>> ListAsync(CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            SELECT PatientId, FullName, BirthDate, DocumentNumber, CreatedAtUtc
            FROM dbo.Patient
            ORDER BY CreatedAtUtc DESC;
            """;

        await using var command = new SqlCommand(commandText, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var patients = new List<Patient>();
        while (await reader.ReadAsync(cancellationToken))
        {
            patients.Add(Patient.Rehydrate(
                reader.GetGuid(0),
                reader.GetString(1),
                DateOnly.FromDateTime(reader.GetDateTime(2)),
                reader.GetString(3),
                reader.GetDateTime(4)));
        }

        return patients;
    }

    public async Task<bool> UpdateAsync(
        Guid patientId,
        string fullName,
        DateOnly birthDate,
        string documentNumber,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            UPDATE dbo.Patient
            SET FullName = @FullName,
                BirthDate = @BirthDate,
                DocumentNumber = @DocumentNumber,
                UpdatedAtUtc = SYSUTCDATETIME()
            WHERE PatientId = @PatientId;
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@PatientId", patientId);
        command.Parameters.AddWithValue("@FullName", fullName);
        command.Parameters.AddWithValue("@BirthDate", birthDate.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@DocumentNumber", documentNumber);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(Guid patientId, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            DELETE FROM dbo.Patient
            WHERE PatientId = @PatientId;
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@PatientId", patientId);
        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }
}
