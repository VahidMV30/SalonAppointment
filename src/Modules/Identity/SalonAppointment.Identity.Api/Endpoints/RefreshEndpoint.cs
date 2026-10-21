namespace SalonAppointment.Identity.Api.Endpoints;

public class RefreshEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", async (ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var refreshToken = httpContext.Request.Cookies["refresh-token"];

            var command = new RefreshCommand(refreshToken);

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
                errors =>
                {
                    httpContext.Response.Cookies.Delete("refresh-token", new CookieOptions { Path = "/api/auth" });
                    return errors.ToProblem();
                });
        });
    }
}