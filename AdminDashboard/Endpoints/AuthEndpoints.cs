using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using Microsoft.AspNetCore.Authentication;

namespace AdminDashboard.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/registration", (User user, UserRepository userRepo) =>
            {
                try
                {
                    userRepo.Add(user);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest();
                }
            });

            app.MapGet("/auth", (HttpContext http) =>
            {
                return http.User.Identity?.IsAuthenticated == true
                    ? Results.Ok(true)
                    : Results.BadRequest();
            });

            app.MapPost("/auth/login", (
                string email,
                string password,
                UserRepository userRepo,
                AuthService authService,
                JwtProvider jwtProvider
            ) =>
            {
                var user = userRepo.GetUserByEmail(email);
                if (user is null || !authService.PasswordVerify(password, user.Password))
                {
                    return Results.BadRequest();
                }

                var token = jwtProvider.GenerateToken(user);
                authService.SignInUser(token);
                return Results.Ok();
            });

            app.MapPost("/auth/logout", async (HttpContext http) =>
            {
                await http.SignOutAsync();
                return Results.Ok(true);
            });
        }
    }
}
