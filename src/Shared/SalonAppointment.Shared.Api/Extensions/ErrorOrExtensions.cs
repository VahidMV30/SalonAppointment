namespace SalonAppointment.Shared.Api.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count == 0) return Results.Problem();

        var first = errors[0];

        var body = new { errors = errors.Select(e => new { e.Code, e.Description }) };

        return first.Type switch
        {
            ErrorType.Validation => Results.BadRequest(body),
            ErrorType.Conflict => Results.Conflict(body),
            ErrorType.NotFound => Results.NotFound(body),
            ErrorType.Unauthorized => Results.Json(body, statusCode: StatusCodes.Status401Unauthorized),
            ErrorType.Forbidden => Results.Json(body, statusCode: StatusCodes.Status403Forbidden),
            ErrorType.Failure => Results.BadRequest(body),
            _ => Results.Json(body, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}