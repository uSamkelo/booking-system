using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookingService.Application.Behaviours
{
    public sealed class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var name = typeof(TRequest).Name;
            logger.LogInformation("Handling {Request}", name);
            var sw = Stopwatch.StartNew();

            try
            {
                var response = await next();
                logger.LogInformation("Handled {Request} in {Ms}ms", name, sw.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
               logger.LogWarning(ex, "{Request} failed after {Ms}ms", name, sw.ElapsedMilliseconds);
               throw;
            }
        }
    }
}