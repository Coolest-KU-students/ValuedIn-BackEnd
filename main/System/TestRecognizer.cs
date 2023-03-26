namespace ValuedInBE.System
{
    public static class TestRecognizer
    {
        private const string testAssemblyName = "xunit.runner";
        private static readonly bool _isTestingEnvironment =
                AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Any(a => a.FullName.ToLowerInvariant().StartsWith(testAssemblyName));
        public static bool IsTestingEnvironment => _isTestingEnvironment;
    }
}
