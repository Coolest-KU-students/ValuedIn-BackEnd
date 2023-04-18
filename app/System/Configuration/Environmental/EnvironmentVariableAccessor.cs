
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.System.Configuration.Environmental
{
    public static class EnvironmentVariableAccessor
    {
        private const string testPrefix = "TEST_";
        private static readonly bool _addPrefix = TestRecognizer.IsTestingEnvironment;
        private static readonly string _prefix = _addPrefix ? testPrefix : string.Empty;

        public static string GetEnvironmentVariable(string environmentVariableName)
        {
            string variable = _prefix + environmentVariableName;
            string? environmentVariable = Environment.GetEnvironmentVariable(variable);
            return environmentVariable ?? throw new EnivronmentVariableMissingException(variable);
        }

    }
}
