using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AdminDashboard.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("/users/clients", (UserRepository userRepo) =>
            {
                var users = userRepo.GetAllUsersWithPaymentsAndTokenBalance();
                return Results.Ok(users);
            });

            app.MapGet("/users", (int id, UserRepository userRepo) =>
            {
                var user = userRepo.GetUserWithPaymentsAndTokenBalance(id);
                return user is null ? Results.BadRequest() : Results.Ok(user);
            });

            app.MapGet("/users/payments", (int userId, int take, UserRepository userRepo) =>
            {
                var user = userRepo.GetUserWithPayments(userId);
                if (user is null || user.Payments.Count < take)
                {
                    return Results.BadRequest();
                }

                var payments = user.Payments
                    .Skip(Math.Max(0, user.Payments.Count - take))
                    .Take(take)
                    .ToList();

                return Results.Ok(payments);
            });

            app.MapDelete("/users/delete", [Authorize] (int id, UserRepository userRepo) =>
            {
                var user = userRepo.GetById(id);
                return user is null ? Results.BadRequest() : Results.Ok();
            });

            app.MapPut("/users/update", [Authorize] (int id, User editedUser, UserRepository userRepo) =>
            {
                var user = userRepo.GetById(id);
                if (user is null)
                {
                    return Results.BadRequest();
                }

                user.Login = editedUser.Login;
                user.Password = editedUser.Password;
                user.Email = editedUser.Email;

                if (editedUser.TokenBalance is not null)
                {
                    user.TokenBalance = editedUser.TokenBalance;
                }

                userRepo.Update(user);
                return Results.Ok();
            });
        }   
    }
}
