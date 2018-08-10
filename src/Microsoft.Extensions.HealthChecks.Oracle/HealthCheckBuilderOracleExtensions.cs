using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace Microsoft.Extensions.HealthChecks
{
    public static class HealthCheckBuilderOracleExtensions
    {
        public static HealthCheckBuilder AddOracleCheck(this HealthCheckBuilder builder, string name, IDbConnection connection)
        {
            Guard.ArgumentNotNull(nameof(builder), builder);

            return AddOracleCheck(builder, name, connection, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddOracleCheck(this HealthCheckBuilder builder, string name, IDbConnection connection, TimeSpan cacheDuration)
        {
            builder.AddCheck($"OracleCheck({name})", async () =>
            {
                try
                {
                    using ((OracleConnection)connection)
                    {
                        using (var command = (OracleCommand)connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1 FROM DUAL";
                            var result = await command.ExecuteNonQueryAsync();
                            if (result == 1)
                            {
                                return HealthCheckResult.Healthy($"OracleCheck({name}): Healthy");
                            }

                            return HealthCheckResult.Unhealthy($"OracleCheck({name}): Unhealthy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"OracleCheck({name}): Exception during check: {ex.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}
