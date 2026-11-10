using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Accessibility;
using Microsoft.Data.SqlClient;

namespace HEJCareSuite.Infrastructure.Accessibility;

public sealed class SqlVlibrasRepository(string connectionString) : IVLibrasService
{
    public async Task<VLibrasTranslation> TranslateTextAsync(
        string text,
        CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = """
            INSERT INTO dbo.VLibrasTranslation
                (VLibrasId, SourceText, Direction, TranslatedText, CreatedAtUtc)
            OUTPUT INSERTED.VLibrasId, INSERTED.SourceText, INSERTED.Direction,
                   INSERTED.TranslatedText, INSERTED.CreatedAtUtc
            VALUES
                (NEWID(), @SourceText, @Direction, @TranslatedText, SYSUTCDATETIME());
            """;

        await using var command = new SqlCommand(commandText, connection);
        command.Parameters.AddWithValue("@SourceText", text);
        command.Parameters.AddWithValue("@Direction", (int)TranslationDirection.TextToLibras);
        command.Parameters.AddWithValue("@TranslatedText", "[mocked translation]");

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return VLibrasTranslation.Rehydrate(
                reader.GetGuid(0),
                reader.GetString(1),
                (TranslationDirection)reader.GetInt32(2),
                reader.GetString(3),
                reader.GetDateTime(4));
        }

        throw new InvalidOperationException("Failed to insert VLIBRAS translation.");
    }
}
