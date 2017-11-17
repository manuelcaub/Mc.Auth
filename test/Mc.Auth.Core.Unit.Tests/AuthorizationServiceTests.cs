using System;
using System.Threading.Tasks;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Mc.Auth.Core.Services;
using NSubstitute;
using Xunit;

namespace Mc.Auth.Core.Unit.Tests
{
    public class AuthorizationServiceTests
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("12345")]
        public async Task CheckIncorrectPassword_ShouldReturnsFalse(string password)
        {
            var authService = new AuthorizationService(_userRepository);
            var checkPassword = await authService
                .CheckPasswordAsync(password, null);
            Assert.False(checkPassword);
        }

        [Fact]
        public async Task CheckCorrectPassword_ShouldReturnsTrue()
        {
            const string username = "username";
            const string password = "password";
            var authService = new AuthorizationService(_userRepository);
            _userRepository
                .FindByEmailAsync(Arg.Is(username))
                .Returns(new User
                {
                    UserName = username,
                    Password = password
                });

            var checkPassword = await authService
                .CheckPasswordAsync(password, username);
            Assert.True(checkPassword);
        }
    }
}
