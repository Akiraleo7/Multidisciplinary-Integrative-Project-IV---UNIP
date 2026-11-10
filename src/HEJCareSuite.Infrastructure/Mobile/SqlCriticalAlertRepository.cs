using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Mobile;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Mobile;

public sealed class SqlCriticalAlertRepository(string connectionString) : ICriticalAlertRepository
{
    public async Task AddAsync(CriticalAlert alert, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.CriticalAlert
                (AlertId, PatientId, Severity, Message, IsAcknowledged,
                TriggeredAtUtc, AcknowledgedAtUtc, AcknowledgedByUserId)
            VALUES
                (@AlertId, @PatientId, @Severity, @Message, @IsAcknowledged,
                @TriggeredAtUtc, @AcknowledgedAtUtc, @AcknowledgedByUserId);
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@AlertId", alert.Id);
        command.Parameters.AddWithValue("@PatientId", alert.PatientId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Severity", (int)alert.Severity);
        command.Parameters.AddWithValue("@Message", alert.Message);
        command.Parameters.AddWithValue("@IsAcknowledged", alert.IsAcknowledged);
        command.Parameters.AddWithValue("@TriggeredAtUtc", alert.TriggeredAtUtc);
        command.Parameters.AddWithValue("@AcknowledgedAtUtc", alert.AcknowledgedAtUtc ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AcknowledgedByUserId", alert.AcknowledgedByUserId ?? (object)DBNull.Value);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CriticalAlert>> ListUnacknowledgedAsync(
        Guid? patientId,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = """
            SELECT AlertId, PatientId, Severity, Message, IsAcknowledged,
                   TriggeredAtUtc, AcknowledgedAtUtc, AcknowledgedByUserId
            FROM dbo.CriticalAlert
            WHERE IsAcknowledged = 0
              AND (@PatientId IS NULL OR PatientId = @PatientId)
            ORDER BY TriggeredAtUtc DESC;
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PatientId", patientId ?? (object)DBNull.Value);

        var alerts = new List<CriticalAlert>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            alerts.Add(CriticalAlert.Rehydrate(
                reader.GetGuid(0),
                reader.IsDBNull(1) ? null : reader.GetGuid(1),
                (AlertSeverity)reader.GetInt32(2),
                reader.GetString(3),
                reader.GetBoolean(4),
                reader.GetDateTime(5),
                reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                reader.IsDBNull(7) ? null : reader.GetGuid(7)));
        }

        return alerts;
    }

    public async Task<bool> AcknowledgeAsync(
        Guid alertId,
        Guid? acknowledgedByUserId,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            UPDATE dbo.CriticalAlert
            SET IsAcknowledged = 1,
                AcknowledgedAtUtc = SYSUTCDATETIME(),
                AcknowledgedByUserId = @AcknowledgedByUserId
            WHERE AlertId = @AlertId
              AND IsAcknowledged = 0;
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@AlertId", alertId);
        command.Parameters.AddWithValue("@AcknowledgedByUserId", acknowledgedByUserId ?? (object)DBNull.Value);

        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }
}
