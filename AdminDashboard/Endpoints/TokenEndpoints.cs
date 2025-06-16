using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;

namespace AdminDashboard.Endpoints
{
    public static class TokenEndpoints
    {
        public static void MapTokenEndpoints(this WebApplication app)
        {
            app.MapGet("/rate/all", (TokenRepository tokenRepo) =>
            {
                return Results.Ok(tokenRepo.GetAll().ToList());
            });

            app.MapGet("/rate", (string nameToken, TokenRepository tokenRepo) =>
            {
                var token = tokenRepo.GetTokenByName(nameToken);
                return token is null ? Results.NotFound() : Results.Ok(token);
            });

            app.MapPost("/rate", (string nameToken, float newRate, TokenRepository tokenRepo) =>
            {
                var token = tokenRepo.GetTokenByName(nameToken);
                if (token is null)
                {
                    return Results.BadRequest();
                }

                tokenRepo.UpdateTokenRate(nameToken, newRate);
                return Results.Ok();
            });
        }
    }
}
