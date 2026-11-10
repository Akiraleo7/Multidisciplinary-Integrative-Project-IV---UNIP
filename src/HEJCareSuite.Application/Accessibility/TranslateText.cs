using HEJCareSuite.Domain.Entities;

namespace HEJCareSuite.Application.Accessibility;

public interface IVLibrasService
{
    Task<VLibrasTranslation> TranslateTextAsync(
        string text,
        CancellationToken cancellationToken);
}

public sealed record TranslateTextCommand(string Text);

public sealed class TranslateText(IVLibrasService service)
{
    public async Task<VLibrasTranslation> ExecuteAsync(
        TranslateTextCommand command,
        CancellationToken cancellationToken)
    {
        return await service.TranslateTextAsync(command.Text, cancellationToken);
    }
}