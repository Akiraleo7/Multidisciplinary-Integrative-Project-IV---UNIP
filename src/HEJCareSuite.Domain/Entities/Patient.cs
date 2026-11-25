namespace HEJCareSuite.Domain.Entities;

public sealed class Patient
{
    private Patient()
    {
    }

    public Patient(string fullName, DateOnly birthDate, string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Patient full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Patient document number is required.", nameof(documentNumber));

        // Validate document number format (CPF or RG)
        string trimmedDoc = documentNumber.Trim();
        if (!IsValidDocumentNumber(trimmedDoc))
        {
            throw new ArgumentException("Invalid document number. CPF must have 11 digits, RG must have between 7 and 9 digits.", nameof(documentNumber));
        }

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        BirthDate = birthDate;
        DocumentNumber = trimmedDoc;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    public static Patient Rehydrate(
        Guid id,
        string fullName,
        DateOnly birthDate,
        string documentNumber,
        DateTime createdAtUtc)
    {
        // For rehydration, we bypass the validation since the data is already in the database
        // We use the private constructor or a slightly different logic
        var patient = new Patient
        {
            Id = id,
            FullName = fullName,
            BirthDate = birthDate,
            DocumentNumber = documentNumber,
            CreatedAtUtc = createdAtUtc
        };

        return patient;
    }

    public void UpdateDetails(string fullName, DateOnly birthDate, string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Patient full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Patient document number is required.", nameof(documentNumber));

        // Validate document number format (CPF or RG)
        string trimmedDoc = documentNumber.Trim();
        if (!IsValidDocumentNumber(trimmedDoc))
        {
            throw new ArgumentException("Invalid document number. CPF must have 11 digits, RG must have between 7 and 9 digits.", nameof(documentNumber));
        }

        FullName = fullName.Trim();
        BirthDate = birthDate;
        DocumentNumber = trimmedDoc;
    }

    /// <summary>
    /// Validates if the document number is a valid CPF (11 digits) or RG (7-9 digits).
    /// </summary>
    /// <param name="documentNumber">The document number to validate (without formatting characters)</param>
    /// <returns>true if valid CPF (11 digits) or RG (7-9 digits), false otherwise</returns>
    private static bool IsValidDocumentNumber(string documentNumber)
    {
        // Remove any non-digit characters for validation
        string digitsOnly = new string(documentNumber.Where(char.IsDigit).ToArray());

        // CPF: must have exactly 11 digits
        if (digitsOnly.Length == 11)
        {
            return IsValidCPF(digitsOnly);
        }

        // RG: must have between 7 and 9 digits
        if (digitsOnly.Length >= 7 && digitsOnly.Length <= 9)
        {
            return true; // RG format is accepted within this range
        }

        return false;
    }

    /// <summary>
    /// Validates a CPF number using the standard CPF validation algorithm.
    /// </summary>
    /// <param name="cpf">The CPF number as a string of 11 digits (without formatting)</param>
    /// <returns>true if the CPF is valid, false otherwise</returns>
    private static bool IsValidCPF(string cpf)
    {
        // Check if all digits are the same (invalid)
        if (cpf.All(d => d == cpf[0]))
        {
            return false;
        }

        // Validate first check digit
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (cpf[i] - '0') * (10 - i);
        }
        int remainder = sum % 11;
        int firstCheckDigit = remainder < 2 ? 0 : 11 - remainder;
        if (firstCheckDigit != cpf[9] - '0')
        {
            return false;
        }

        // Validate second check digit
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (cpf[i] - '0') * (11 - i);
        }
        remainder = sum % 11;
        int secondCheckDigit = remainder < 2 ? 0 : 11 - remainder;
        if (secondCheckDigit != cpf[10] - '0')
        {
            return false;
        }

        return true;
    }
}
