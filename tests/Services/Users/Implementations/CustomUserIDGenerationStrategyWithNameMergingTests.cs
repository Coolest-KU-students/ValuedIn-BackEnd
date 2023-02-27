using Xunit;
using ValuedInBE.Services.Users.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.Controllers;

namespace ValuedInBE.Services.Users.Implementations.Tests
{
    public class CustomUserIDGenerationStrategyWithNameMergingTests
    {

        private readonly Mock<ILogger<CustomUserIDGenerationStrategyWithNameMerging>> _mockLogger = new();

        [Fact()]
        public void GenerateUserIDTest()
        {
            string firstName = "FirstName";
            string lastName = "LastName";
            int sameNameCount = 10;
            int expectedGeneratedPartLength = 8;
            CustomUserIDGenerationStrategyWithNameMerging strategy = new(_mockLogger.Object);
            string generatedId = strategy.GenerateUserIDForNewUser(firstName, lastName, sameNameCount);
            Assert.NotNull(generatedId);
            Assert.NotEmpty(generatedId);
            Assert.Contains(firstName, generatedId);
            Assert.Contains(lastName, generatedId);
            Assert.Contains(sameNameCount.ToString(), generatedId);
            Assert.Equal(generatedId.Split('-')[1].Length, expectedGeneratedPartLength);
        }
    }
}