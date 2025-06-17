using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AdminDashboard.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/registration", (User user, UserRepository userRepository) =>
            {
                try
                {
                    userRepository.Add(user);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest();
                }
            });

            app.MapGet("/auth", (HttpContext http, UserRepository userRepository) =>
            {
                if (http.User.Identity?.IsAuthenticated == true)
                {
                    var user = userRepository.GetById((int.Parse)(http.User.Claims.First(c => c.Type == "id").Value));
                    return Results.Ok(user);
                }
                else
                {
                    return Results.BadRequest();
                }
            });

            app.MapPost("/login", (
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
                return Results.Ok(token);
            });

            app.MapPost("/logout", async (HttpContext http) =>
            {
                await http.SignOutAsync();
                return Results.Ok(true);
            });
        }
    }
}
