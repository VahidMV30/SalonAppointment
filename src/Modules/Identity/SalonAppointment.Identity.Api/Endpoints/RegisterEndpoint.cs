namespace SalonAppointment.Identity.Api.Endpoints;

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register",
            async (RegisterRequest request, ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var command = new RegisterCommand(request.FullName, request.Mobile, request.Password);

                var result = await sender.Send(command, cancellationToken);

                return result.Match(
                    value =>
                    {
                        httpContext.Response.Cookies.Append("refresh-token", value.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = new DateTimeOffset(value.RefreshTokenExpiresAt, TimeSpan.Zero),
                            Path = "/api/auth"
                        });

                        return Results.Ok(new
                        {
                            value.AccessToken,
                            value.AccessTokenExpiresAt,
                            value.RefreshTokenExpiresAt
                        });
                    },
                    errors => errors.ToProblem());
            });
    }
}