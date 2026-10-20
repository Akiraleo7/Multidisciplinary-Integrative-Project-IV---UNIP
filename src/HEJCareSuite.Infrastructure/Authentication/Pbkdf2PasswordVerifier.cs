using System.Security.Cryptography;
using HEJCareSuite.Application.Authentication;

namespace HEJCareSuite.Infrastructure.Authentication;

public sealed class Pbkdf2PasswordVerifier : IPasswordVerifier
{
    private const int Iterations = 100_000;

    public bool Verify(string password, string encodedHash, string encodedSalt)
    {
        try
        {
            var salt = Convert.FromBase64String(encodedSalt);
            var expectedHash = Convert.FromBase64String(encodedHash);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
