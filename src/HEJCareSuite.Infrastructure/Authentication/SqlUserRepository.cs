using HEJCareSuite.Application.Authentication;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Authentication;

public sealed class SqlUserRepository(string connectionString) : IUserRepository
{
    public async Task<UserAccount?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            SELECT UserId, Email, PasswordHash, PasswordSalt, RoleName
            FROM dbo.UserAccount
            WHERE Email = @Email AND IsActive = 1;
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@Email", email);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken)
            ? new UserAccount(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4))
            : null;
    }
}
