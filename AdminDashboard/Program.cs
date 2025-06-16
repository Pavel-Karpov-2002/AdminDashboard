using AdminDashboard.DbStuff;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
//var connectionString = builder.Configuration.GetConnectionString("localDb");

builder.Services.AddDbContext<SocialNetworkWebDbContext>(options => 
               options.UseSqlite(builder.Configuration.GetConnectionString("localDb")));
builder.Services.AddScoped<UserBuilder>();
builder.Services.AddScoped<TokenBuilder>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenRepository>();

var app = builder.Build();

//SeedExtention.Seed(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
