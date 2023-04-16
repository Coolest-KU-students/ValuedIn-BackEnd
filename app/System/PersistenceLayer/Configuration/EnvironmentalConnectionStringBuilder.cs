using Microsoft.Data.SqlClient;
using ValuedInBE.System.Exceptions;

namespace ValuedInBE.System.PersistenceLayer.Configuration
{
    public static class EnvironmentalConnectionStringBuilder
    {
        public static string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = GetFromEnvironmentOrThrow("MSSQL_HOST"),
                InitialCatalog = GetFromEnvironmentOrThrow("MSSQL_DB"),
                UserID = GetFromEnvironmentOrThrow("MSSQL_USER"),
                Password = GetFromEnvironmentOrThrow("MSSQL_PASSWORD"),
            };

            return builder.ConnectionString;

        }

        private static string GetFromEnvironmentOrThrow(string environmentVariable)
        {
            return Environment.GetEnvironmentVariable(environmentVariable)
            ?? throw new EnivronmentVariableMissingException(environmentVariable);

        }

    }
}
