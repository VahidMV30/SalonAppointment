namespace SalonAppointment.Identity.Api.Endpoints;

public class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetAllUsersQuery(), cancellationToken);

            return Results.Ok(result);
        }).RequireAuthorization(policy => policy.RequireRole(ApplicationRoles.Admin));
    }
}