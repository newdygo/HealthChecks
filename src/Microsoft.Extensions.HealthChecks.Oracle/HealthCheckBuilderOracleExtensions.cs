﻿namespace Microsoft.Extensions.HealthChecks
{
    using Microsoft.Extensions.HealthChecks.Infra;
    using System;
    using System.Data;

    public static class HealthCheckBuilderOracleExtensions
    {
        public static HealthCheckBuilder AddOracleCheck(this HealthCheckBuilder builder, string name, IDbConnection connection)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddOracleCheck(builder, name, connection, builder.DefaultCacheDuration);
        }

        private static HealthCheckBuilder AddOracleCheck(this HealthCheckBuilder builder, string name, IDbConnection connection, TimeSpan cacheDuration)
        {
            builder.AddCheck($"OracleCheck({name})", () =>
            {
                try
                {
                    using (connection)
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = "SELECT 1 FROM DUAL";
                            var result = command.ExecuteNonQuery();
                            if (result == -1)
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
