using AdminDashboard.DbStuff;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = AuthService.AUTH_KEY;
    })
    .AddCookie(AuthService.AUTH_KEY, option =>
    {
        option.AccessDeniedPath = "/auth/deny";
        option.LoginPath = "/auth/Login";
    });

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SocialNetworkWebDbContext>(options => 
               options.UseSqlite(builder.Configuration.GetConnectionString("localDb")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var typeOfBaseRepository = typeof(BaseRepository<>);
Assembly
    .GetAssembly(typeOfBaseRepository)
    .GetTypes()
    .Where(x => x.BaseType?.IsGenericType ?? false
        && x.BaseType.GetGenericTypeDefinition() == typeOfBaseRepository)
    .ToList()
    .ForEach(repositoryType => builder.Services.AddScoped(repositoryType));

// DI service
var typeOfBaseServices = typeof(IService);
AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeOfBaseServices.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
    .ToList()
    .ForEach(serviceType => builder.Services.AddScoped(serviceType));

var app = builder.Build();

SeedExtention.Seed(app);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
