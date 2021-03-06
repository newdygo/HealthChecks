﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.HealthChecks.Infra;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.HealthChecks
{
    public static partial class HealthCheckBuilderExtensions
    {
        // Numeric checks

        public static HealthCheckBuilder AddMinValueCheck<T>(this HealthCheckBuilder builder, string name, T minValue, Func<T> currentValueFunc) where T : IComparable<T>
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddMinValueCheck(builder, name, minValue, currentValueFunc, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddMinValueCheck<T>(this HealthCheckBuilder builder, string name, T minValue, Func<T> currentValueFunc, TimeSpan cacheDuration)
            where T : IComparable<T>
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);
            HealthGuard.ArgumentNotNullOrEmpty(nameof(name), name);
            HealthGuard.ArgumentNotNull(nameof(currentValueFunc), currentValueFunc);

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                var status = currentValue.CompareTo(minValue) >= 0 ? CheckStatus.Healthy : CheckStatus.Unhealthy;
                return HealthCheckResult.FromStatus(
                    status,
                    $"min={minValue}, current={currentValue}",
                    new Dictionary<string, object> { { "min", minValue }, { "current", currentValue } }
                );
            }, cacheDuration);

            return builder;
        }

        public static HealthCheckBuilder AddMaxValueCheck<T>(this HealthCheckBuilder builder, string name, T maxValue, Func<T> currentValueFunc) where T : IComparable<T>
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);

            return AddMaxValueCheck(builder, name, maxValue, currentValueFunc, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddMaxValueCheck<T>(this HealthCheckBuilder builder, string name, T maxValue, Func<T> currentValueFunc, TimeSpan cacheDuration)
            where T : IComparable<T>
        {
            HealthGuard.ArgumentNotNull(nameof(builder), builder);
            HealthGuard.ArgumentNotNullOrEmpty(nameof(name), name);
            HealthGuard.ArgumentNotNull(nameof(currentValueFunc), currentValueFunc);

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                var status = currentValue.CompareTo(maxValue) <= 0 ? CheckStatus.Healthy : CheckStatus.Unhealthy;
                return HealthCheckResult.FromStatus(
                    status,
                    $"max={maxValue}, current={currentValue}",
                    new Dictionary<string, object> { { "max", maxValue }, { "current", currentValue } }
                );
            }, cacheDuration);

            return builder;
        }
    }
}
