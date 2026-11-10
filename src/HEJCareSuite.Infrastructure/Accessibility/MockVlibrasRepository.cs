using HEJCareSuite.Domain.Entities;
using HEJCareSuite.Application.Accessibility;

namespace HEJCareSuite.Infrastructure.Accessibility;

public sealed class MockVlibrasRepository : IVLibrasService
{
    public Task<VLibrasTranslation> TranslateTextAsync(
        string text,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new VLibrasTranslation(
            "[mocked translation]",
            TranslationDirection.TextToLibras,
            text));
    }
}