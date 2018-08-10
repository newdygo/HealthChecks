namespace Microsoft.AspNet.HealthChecks
{
    using Microsoft.Extensions.HealthChecks;
    using Microsoft.Extensions.HealthChecks.Infra;
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public class HealthCheckHandler : HttpTaskAsyncHandler
    {
        private static TimeSpan _timeout = TimeSpan.FromSeconds(10);
        
        public static TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                HealthGuard.ArgumentValid(value > TimeSpan.Zero, nameof(Timeout), "Health check timeout must be a positive time span.");

                _timeout = value;
            }
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var timeoutTokenSource = new CancellationTokenSource(Timeout);
            var result = await GlobalHealthChecks.Service.CheckHealthAsync(timeoutTokenSource.Token);
            var status = result.CheckStatus;

            if (status != CheckStatus.Healthy)
            {
                context.Response.StatusCode = 503;
            }

            context.Response.Headers.Add("content-type", "application/json");
            context.Response.Write(JsonConvert.SerializeObject(new { status = status.ToString() }));
        }
    }
}
