namespace SalonAppointment.Shared.Api.ExceptionHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException)
        {
            logger.LogInformation("Request was cancelled: {Path}", httpContext.Request.Path);
            httpContext.Response.StatusCode = 499;
            return true;
        }

        var isClientError = exception is ArgumentException;

        if (isClientError) logger.LogWarning(exception, "Invalid input: {Message}", exception.Message);
        else logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (status, title, detail) = isClientError
            ? (StatusCodes.Status400BadRequest, "ورودی نامعتبر", "اطلاعات ارسال شده معتبر نیست.")
            : (StatusCodes.Status500InternalServerError, "خطای داخلی سرور", "خطای داخلی سرور رخ داده است.");

        httpContext.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier }
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }
}