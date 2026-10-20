using HEJCareSuite.Application.Authentication;
using Xunit;

namespace HEJCareSuite.Application.Tests;

public sealed class AuthenticationTests
{
    [Fact]
    public async Task ExecuteAsync_ValidCredentials_ReturnsUser()
    {
        var user = new UserAccount(
            Guid.NewGuid(),
            "admin@hejcare.local",
            "hash",
            "salt",
            "Administrator");
        var useCase = new AuthenticateUser(
            new FakeUserRepository(user),
            new FakePasswordVerifier(true));

        var result = await useCase.ExecuteAsync(
            new AuthenticateUserCommand("admin@hejcare.local", "password"),
            CancellationToken.None);

        Assert.Equal(user, result);
    }

    [Fact]
    public async Task ExecuteAsync_InvalidCredentials_ReturnsNull()
    {
        var useCase = new AuthenticateUser(
            new FakeUserRepository(null),
            new FakePasswordVerifier(false));

        var result = await useCase.ExecuteAsync(
            new AuthenticateUserCommand("admin@hejcare.local", "wrong"),
            CancellationToken.None);

        Assert.Null(result);
    }

    private sealed class FakeUserRepository(UserAccount? user) : IUserRepository
    {
        public Task<UserAccount?> FindByEmailAsync(
            string email,
            CancellationToken cancellationToken) =>
            Task.FromResult(user);
    }

    private sealed class FakePasswordVerifier(bool result) : IPasswordVerifier
    {
        public bool Verify(string password, string encodedHash, string encodedSalt) => result;
    }
}
