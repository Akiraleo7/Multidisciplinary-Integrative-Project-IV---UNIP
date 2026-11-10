namespace HEJCareSuite.Domain.Entities;

public enum TranslationDirection
{
    LibrasToText = 1,
    TextToLibras = 2
}

public sealed class VLibrasTranslation
{
    private VLibrasTranslation() { }

    public VLibrasTranslation(
        string sourceText,
        TranslationDirection direction,
        string translatedText)
    {
        if (string.IsNullOrWhiteSpace(sourceText))
            throw new ArgumentException("Source text is required.", nameof(sourceText));
        if (string.IsNullOrWhiteSpace(translatedText))
            throw new ArgumentException("Translated text is required.", nameof(translatedText));

        Id = Guid.NewGuid();
        SourceText = sourceText.Trim();
        Direction = direction;
        TranslatedText = translatedText.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static VLibrasTranslation Rehydrate(
        Guid id,
        string sourceText,
        TranslationDirection direction,
        string translatedText,
        DateTime createdAtUtc)
    {
        return new VLibrasTranslation
        {
            Id = id,
            SourceText = sourceText,
            Direction = direction,
            TranslatedText = translatedText,
            CreatedAtUtc = createdAtUtc
        };
    }

    public Guid Id { get; private set; }
    public string SourceText { get; set; } = string.Empty;
    public TranslationDirection Direction { get; set; }
    public string TranslatedText { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
