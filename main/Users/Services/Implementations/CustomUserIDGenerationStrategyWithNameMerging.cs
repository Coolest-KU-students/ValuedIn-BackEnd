using System.Text;
using ValuedInBE.Users.Models.DTOs.Request;

namespace ValuedInBE.Users.Services.Implementations
{
    public class CustomUserIDGenerationStrategyWithNameMerging : IUserIDGenerationStrategy
    {
        private const int userIDCalculatedSegmentLength = 8;
        private readonly ILogger<CustomUserIDGenerationStrategyWithNameMerging> _logger;

        public CustomUserIDGenerationStrategyWithNameMerging(ILogger<CustomUserIDGenerationStrategyWithNameMerging> logger)
        {
            _logger = logger;
        }

        public Task<string> GenerateUserIDForNewUserAsync(NewUser newUser, int sameNameRepeatCount)
        {
            _logger.LogTrace("Generating User ID for user with login {login}", newUser.Login);
            return Task.FromResult(GenerateUserIDForNewUser(newUser.FirstName, newUser.LastName, sameNameRepeatCount));
        }

        public string GenerateUserIDForNewUser(string firstName, string lastName, int sameNameUserCount)
        {
            int firstNameIndex = 0;
            int lastNameIndex = 0;
            int offsetBy = (int)(DateTime.Now.Ticks % (sameNameUserCount + 1)) % userIDCalculatedSegmentLength;
            int longestWordCount = Math.Max(Math.Max(firstName.Length, lastName.Length), userIDCalculatedSegmentLength);
            StringBuilder calculatedSymbols = new();
            for (int i = 0; i < longestWordCount; i++)
            {
                calculatedSymbols.Append(GetAverageAndOffset(firstName[firstNameIndex], lastName[lastNameIndex], offsetBy));
                firstNameIndex = ++firstNameIndex >= firstName.Length ? 0 : firstNameIndex;
                lastNameIndex = ++lastNameIndex >= lastName.Length ? 0 : lastNameIndex;
            }
            string calculatedSegment = calculatedSymbols.ToString();
            for (int i = 0; calculatedSegment.Length > userIDCalculatedSegmentLength; i++)
            {
                if (i > calculatedSegment.Length) i = 0;
                char firstSymbol = calculatedSegment[i];
                char lastSymbol = calculatedSegment[^1];
                calculatedSegment = calculatedSegment.Remove(i, 1);
                calculatedSegment = calculatedSegment.Remove(calculatedSegment.Length - 1, 1);
                calculatedSegment = $"{GetAverageAndOffset(firstSymbol, lastSymbol, -offsetBy)}{calculatedSegment}";
            }

            return $"{firstName}.{lastName}.{sameNameUserCount}-{calculatedSegment}";
        }

        private static char GetAverageAndOffset(char a, char b, int offset) => (char)((a + b) / 2 + offset);
    }
}
