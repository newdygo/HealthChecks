﻿namespace Microsoft.Extensions.HealthChecks.Infra
{
    using System;

    public static class HealthGuard
    {
        public static void ArgumentNotNull(string argumentName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argumentName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value cannot be an empty string.", argumentName);
            }
        }

        public static void ArgumentValid(bool valid, string argumentName, string exceptionMessage)
        {
            if (!valid)
            {
                throw new ArgumentException(exceptionMessage, argumentName);
            }
        }

        public static void OperationValid(bool valid, string exceptionMessage)
        {
            if (!valid)
            {
                throw new InvalidOperationException(exceptionMessage);
            }
        }
    }
}
