using HEJCareSuite.Application.Triage;
using HEJCareSuite.Domain.Entities;
using Microsoft.Data.SqlClient;
using TriageEntity = HEJCareSuite.Domain.Entities.Triage;

namespace HEJCareSuite.Infrastructure.Triage;

public sealed class SqlTriageRepository(string connectionString) : ITriageRepository
{
    public async Task AddAsync(TriageEntity triage, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.Triage
                (TriageId, PatientId, RiskLevel, ChiefComplaint, RecordedAtUtc)
            VALUES
                (@TriageId, @PatientId, @RiskLevel, @ChiefComplaint, @RecordedAtUtc);
            """;
        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@TriageId", triage.Id);
        command.Parameters.AddWithValue("@PatientId", triage.PatientId);
        command.Parameters.AddWithValue("@RiskLevel", (int)triage.RiskLevel);
        command.Parameters.AddWithValue("@ChiefComplaint", triage.ChiefComplaint);
        command.Parameters.AddWithValue("@RecordedAtUtc", triage.RecordedAtUtc);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TriageEntity>> ListByPatientAsync(
        Guid patientId,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        const string commandText = """
            SELECT TriageId, PatientId, RiskLevel, ChiefComplaint, RecordedAtUtc
            FROM dbo.Triage
            WHERE PatientId = @PatientId
            ORDER BY RecordedAtUtc DESC;
            """;
        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@PatientId", patientId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var triages = new List<TriageEntity>();
        while (await reader.ReadAsync(cancellationToken))
        {
            triages.Add(TriageEntity.Rehydrate(
                reader.GetGuid(0),
                reader.GetGuid(1),
                (TriageRiskLevel)reader.GetInt32(2),
                reader.GetString(3),
                reader.GetDateTime(4)));
        }
        return triages;
    }
}
