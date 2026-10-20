namespace HEJCareSuite.Application.Authentication;

public sealed record AuthenticateUserCommand(string Email, string Password);

public sealed class AuthenticateUser(
    IUserRepository userRepository,
    IPasswordVerifier passwordVerifier)
{
    public async Task<UserAccount?> ExecuteAsync(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Email)
            || string.IsNullOrWhiteSpace(command.Password))
            return null;

        var user = await userRepository.FindByEmailAsync(
            command.Email.Trim(),
            cancellationToken);

        return user is not null
            && passwordVerifier.Verify(command.Password, user.PasswordHash, user.PasswordSalt)
            ? user
            : null;
    }
}

public interface IPasswordVerifier
{
    bool Verify(string password, string encodedHash, string encodedSalt);
}
