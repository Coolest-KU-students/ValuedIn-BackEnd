namespace ValuedInBE.System.Exceptions
{
    public class EnivronmentVariableMissingException : SystemSetupException
    {
        private const string baseString = "Could not find '{0}' in the environment variables. Ensure the variable is set or the environment file created";

        public EnivronmentVariableMissingException(string environmentVariable) : base(string.Format(baseString, environmentVariable))
        {
        } 
    }
}
