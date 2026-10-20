namespace HEJCareSuite.Application.Authentication;

public sealed record UserAccount(
    Guid Id,
    string Email,
    string PasswordHash,
    string PasswordSalt,
    string Role);

public interface IUserRepository
{
    Task<UserAccount?> FindByEmailAsync(string email, CancellationToken cancellationToken);
}
