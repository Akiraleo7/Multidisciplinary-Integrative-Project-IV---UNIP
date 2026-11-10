using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Mobile;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Mobile;

public sealed class SqlStaffTaskRepository(string connectionString) : IStaffTaskRepository
{
    public async Task AddAsync(StaffTask task, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.StaffTask
                (TaskId, PatientId, Title, Description, Priority, AssignedRole, DueDateUtc, CreatedAtUtc)
            VALUES
                (@TaskId, @PatientId, @Title, @Description, @Priority, @AssignedRole, @DueDateUtc, @CreatedAtUtc);
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@TaskId", task.Id);
        command.Parameters.AddWithValue("@PatientId", task.PatientId);
        command.Parameters.AddWithValue("@Title", task.Title);
        command.Parameters.AddWithValue("@Description", task.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Priority", (int)task.Priority);
        command.Parameters.AddWithValue("@AssignedRole", task.AssignedRole);
        command.Parameters.AddWithValue("@DueDateUtc", task.DueDateUtc ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@CreatedAtUtc", task.CreatedAtUtc);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<StaffTask>> ListByPatientAsync(
        Guid patientId,
        bool? pendingOnly,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var sql = """
            SELECT TaskId, PatientId, Title, Description, Priority, IsCompleted,
                   AssignedRole, DueDateUtc, CreatedAtUtc, CompletedAtUtc
            FROM dbo.StaffTask
            WHERE PatientId = @PatientId
              AND (@PendingOnly = 0 OR IsCompleted = 0)
            ORDER BY CreatedAtUtc DESC;
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PatientId", patientId);
        command.Parameters.AddWithValue("@PendingOnly", pendingOnly ?? false);

        var tasks = new List<StaffTask>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            tasks.Add(StaffTask.Rehydrate(
                reader.GetGuid(0),
                reader.GetGuid(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                (TaskPriority)reader.GetInt32(4),
                reader.GetBoolean(5),
                reader.GetString(6),
                reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                reader.GetDateTime(8),
                reader.IsDBNull(9) ? null : reader.GetDateTime(9)));
        }

        return tasks;
    }

    public async Task<bool> CompleteAsync(
        Guid taskId,
        Guid patientId,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            UPDATE dbo.StaffTask
            SET IsCompleted = 1,
                CompletedAtUtc = SYSUTCDATETIME()
            WHERE TaskId = @TaskId
              AND PatientId = @PatientId
              AND IsCompleted = 0;
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@TaskId", taskId);
        command.Parameters.AddWithValue("@PatientId", patientId);

        return await command.ExecuteNonQueryAsync(cancellationToken) == 1;
    }
}
