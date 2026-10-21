namespace SalonAppointment.Shared.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse> where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("[START] {Request}", requestName);

        var startTime = Stopwatch.GetTimestamp();

        var response = await next(cancellationToken);

        var elapsed = Stopwatch.GetElapsedTime(startTime);

        if (response is IErrorOr { IsError: true } errorOr)
        {
            var errorCodes = string.Join(", ", errorOr.Errors!.Select(e => e.Code));
            logger.LogWarning("[FAILED] {Request} - Errors={Errors}", requestName, errorCodes);
        }
        else
        {
            logger.LogInformation("[END] {Request} Succeeded In {Elapsed} MS", requestName, elapsed.TotalMilliseconds);
        }

        if (elapsed.TotalSeconds > 3)
            logger.LogWarning("[PERFORMANCE] {Request} Took {Elapsed} Seconds", requestName, elapsed.TotalSeconds);

        return response;
    }
}