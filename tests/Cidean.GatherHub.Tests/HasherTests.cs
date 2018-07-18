using Cidean.GatherHub.Core.Helpers;
using System;
using Xunit;

namespace Cidean.GatherHub.Tests
{
    public class HasherTests
    {
        [Fact]
        public void IsValid_ValidPassword_ReturnsTrue()
        {
            var rawPassword = "password";
            var hashedPassword = Hasher.Generate(rawPassword, 100);

            Assert.True(Hasher.IsValid(rawPassword, hashedPassword));
            
        }
        [Fact]
        public void IsValid_InvalidPassword_ReturnsFalse()
        {
            var rawPassword = "password";
            var hashedPassword = Hasher.Generate(rawPassword, 100);

            Assert.False(Hasher.IsValid("invalidpass", hashedPassword));

        }

        [Fact]
        public void Generate_Generate100Iterations_Returns3DelimString()
        {
            var rawPassword = "password";
            var hashedPassword = Hasher.Generate(rawPassword, 100);

            Assert.True(hashedPassword.Split("|").Length==3);

        }
    }
}
