using AdminDashboard.DbStuff;
using AdminDashboard.DbStuff.Repositories;
using AdminDashboard.Endpoints;
using AdminDashboard.Services;
using AdminDashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
        });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SECRET_KEY))
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["JWT"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var typeOfBaseServices = typeof(IService);
AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeOfBaseServices.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
    .ToList()
    .ForEach(serviceType => builder.Services.AddScoped(serviceType));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// Подключаем endpoint'ы
app.MapAuthEndpoints();
app.MapTokenEndpoints();
app.MapUserEndpoints();

app.MapControllers();

app.Run();
