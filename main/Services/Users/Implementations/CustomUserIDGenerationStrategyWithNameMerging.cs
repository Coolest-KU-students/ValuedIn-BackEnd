using System.Text;
using ValuedInBE.Models.DTOs.Requests.Users;

namespace ValuedInBE.Services.Users.Implementations
{
    public class CustomUserIDGenerationStrategyWithNameMerging : IUserIDGenerationStrategy
    {
        private const int userIDCalculatedSegmentLength = 8;
        public Task<string> GenerateUserIDForNewUser(NewUser newUser, int sameNameRepeatCount)
        {
            return Task.FromResult(GenerateUserIDForNewUser(newUser.FirstName, newUser.LastName, sameNameRepeatCount));
        }

        public string GenerateUserIDForNewUser(string firstName, string lastName, int sameNameUserCount)
        {
            int firstNameIndex = 0;
            int lastNameIndex = 0;
            int offsetBy = (int) (DateTime.Now.Ticks % (sameNameUserCount+1)) % userIDCalculatedSegmentLength;
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
                char firstSymbol = calculatedSegment[i];
                char lastSymbol = calculatedSegment[^1];
                calculatedSegment = $"{GetAverageAndOffset(firstSymbol, lastSymbol, -offsetBy)}{calculatedSegment.Trim(firstSymbol, lastSymbol)}";
            }

            return $"{firstName}.{lastName}.{sameNameUserCount}-{calculatedSegment}";
        }

        private static char GetAverageAndOffset(char a, char b, int offset) => (char)((a + b) / 2 + offset); 
    }
}
