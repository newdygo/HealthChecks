namespace Microsoft.Extensions.HealthChecks
{
    using Microsoft.Extensions.HealthChecks.Infra;
    using System;

    public static class GlobalHealthChecks
    {
        private static IHealthCheckService _service;

        public static IHealthCheckService Service
        {
            get
            {
                HealthGuard.OperationValid(_service != null, "You must call Build before retrieving the service.");

                return _service;
            }
        }
        

        public static void Build(Action<HealthCheckBuilder> buildout)
        {
            HealthGuard.ArgumentNotNull(nameof(buildout), buildout);
            HealthGuard.OperationValid(_service == null, "You may only call Build once.");

            var builder = new HealthCheckBuilder();
            buildout(builder);

            _service = new HealthCheckService(builder, new NoOpServiceProvider());
        }

        class NoOpServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType) => null;
        }
    }
}
