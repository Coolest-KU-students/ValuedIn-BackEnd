using Microsoft.Data.SqlClient;

namespace ValuedInBE.System.Configuration.Environmental
{
    public static class EnvironmentalConnectionStringBuilder
    {
        public static string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = EnvironmentVariableAccessor.GetEnvironmentVariable("MSSQL_HOST"),
                InitialCatalog = EnvironmentVariableAccessor.GetEnvironmentVariable("MSSQL_DB"),
                UserID = EnvironmentVariableAccessor.GetEnvironmentVariable("MSSQL_USER"),
                Password = EnvironmentVariableAccessor.GetEnvironmentVariable("MSSQL_PASSWORD"),
            };

            return builder.ConnectionString;
        }
    }
}
