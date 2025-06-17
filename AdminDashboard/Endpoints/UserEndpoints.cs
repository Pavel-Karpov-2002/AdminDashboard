using AdminDashboard.DbStuff.Models;
using AdminDashboard.DbStuff.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("/clients", (UserRepository userRepository) =>
            {
                var users = userRepository.GetAllUsersWithPaymentsAndTokenBalance();
                return Results.Ok(users);
            });

            app.MapGet("/user", (int id, UserRepository userRepository) =>
            {
                var user = userRepository.GetUserWithPaymentsAndTokenBalance(id);
                return user is null ? Results.BadRequest() : Results.Ok(user);
            });

            app.MapGet("/user/payments", (int userId, int take, UserRepository userRepository) =>
            {
                var user = userRepository.GetUserWithPayments(userId);
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

            app.MapDelete("/user/delete", [Authorize] (int id, UserRepository userRepository) =>
            {
                var user = userRepository.GetById(id);
                if (user is null)
                {
                    return Results.BadRequest();
                }
                else
                {
                    userRepository.DeleteById(id);
                    return Results.Ok();
                }
            });

            app.MapPut("/user/update", [Authorize] (int id, User editedUser, UserRepository userRepository) =>
            {
                userRepository.UpdateUser(editedUser);
                return Results.Ok();
            });
        }   
    }
}
