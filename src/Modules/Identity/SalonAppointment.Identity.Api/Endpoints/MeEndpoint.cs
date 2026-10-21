namespace SalonAppointment.Identity.Api.Endpoints;

public class MeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new MeQuery(), cancellationToken);

            return result.Match(Results.Ok, errors => errors.ToProblem());
        }).RequireAuthorization();
    }
}