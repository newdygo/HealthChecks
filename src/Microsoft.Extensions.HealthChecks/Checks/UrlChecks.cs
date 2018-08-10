// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.HealthChecks.Infra;
using Microsoft.Extensions.HealthChecks.Internal;

namespace Microsoft.Extensions.HealthChecks
{
    public static partial class HealthCheckBuilderExtensions
    {
        // Default URL check

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddUrlCheck(builder, url, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url, TimeSpan cacheDuration)
            => AddUrlCheck(builder, url, response => UrlChecker.DefaultUrlCheck(response), cacheDuration);

        // Func returning IHealthCheckResult

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url, Func<HttpResponseMessage, IHealthCheckResult> checkFunc)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddUrlCheck(builder, url, checkFunc, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url,
                                                     Func<HttpResponseMessage, IHealthCheckResult> checkFunc,
                                                     TimeSpan cacheDuration)
        {
            HealthGuard.ArgumentNotNull(nameof(checkFunc), checkFunc);

            return AddUrlCheck(builder, url, response => new ValueTask<IHealthCheckResult>(checkFunc(response)), cacheDuration);
        }

        // Func returning Task<IHealthCheckResult>

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url, Func<HttpResponseMessage, Task<IHealthCheckResult>> checkFunc)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddUrlCheck(builder, url, checkFunc, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url,
                                                     Func<HttpResponseMessage, Task<IHealthCheckResult>> checkFunc,
                                                     TimeSpan cacheDuration)
        {
            HealthGuard.ArgumentNotNull(nameof(checkFunc), checkFunc);

            return AddUrlCheck(builder, url, response => new ValueTask<IHealthCheckResult>(checkFunc(response)), cacheDuration);
        }

        // Func returning ValueTask<IHealthCheckResult>

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url, Func<HttpResponseMessage, ValueTask<IHealthCheckResult>> checkFunc)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddUrlCheck(builder, url, checkFunc, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddUrlCheck(this HealthCheckBuilder builder, string url,
                                                     Func<HttpResponseMessage, ValueTask<IHealthCheckResult>> checkFunc,
                                                     TimeSpan cacheDuration)
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);
            HealthGuard.ArgumentNotNullOrEmpty(nameof(url), url);
            HealthGuard.ArgumentNotNull(nameof(checkFunc), checkFunc);

            var urlCheck = new UrlChecker(checkFunc, url);
            builder.AddCheck($"UrlCheck({url})", () => urlCheck.CheckAsync(), cacheDuration);
            return builder;
        }
    }
}
