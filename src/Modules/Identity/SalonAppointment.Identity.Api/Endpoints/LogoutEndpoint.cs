namespace SalonAppointment.Identity.Api.Endpoints;

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/logout", async (ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var refreshToken = httpContext.Request.Cookies["refresh-token"];

            await sender.Send(new LogoutCommand(refreshToken), cancellationToken);

            httpContext.Response.Cookies.Delete("refresh-token", new CookieOptions { Path = "/api/auth" });

            return Results.Ok();
        });
    }
}