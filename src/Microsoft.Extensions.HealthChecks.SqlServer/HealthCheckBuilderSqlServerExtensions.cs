namespace Microsoft.Extensions.HealthChecks
{
    using Microsoft.Extensions.HealthChecks.Infra;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    
    public static class HealthCheckBuilderSqlServerExtensions
    {
        public static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, IDbConnection connection)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddSqlCheck(builder, name, connection, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, string connectionString)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddSqlCheck(builder, name, connectionString, builder.DefaultCacheDuration);
        }

        private static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, string connectionString, TimeSpan cacheDuration)
        {
            builder.AddCheck($"SqlCheck({name})", async () =>
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1";
                            var result = (int)await command.ExecuteScalarAsync().ConfigureAwait(false);
                            if (result == 1)
                            {
                                return HealthCheckResult.Healthy($"SqlCheck({name}): Healthy");
                            }

                            return HealthCheckResult.Unhealthy($"SqlCheck({name}): Unhealthy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"SqlCheck({name}): Exception during check: {ex.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }

        private static HealthCheckBuilder AddSqlCheck(this HealthCheckBuilder builder, string name, IDbConnection connection, TimeSpan cacheDuration)
        {
            builder.AddCheck($"SqlCheck({name})", () =>
            {
                try
                {
                    using (connection)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1";
                            var result = (int)command.ExecuteScalar();
                            if (result == 1)
                            {
                                return HealthCheckResult.Healthy($"SqlCheck({name}): Healthy");
                            }

                            return HealthCheckResult.Unhealthy($"SqlCheck({name}): Unhealthy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"SqlCheck({name}): Exception during check: {ex.GetType().FullName}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}
