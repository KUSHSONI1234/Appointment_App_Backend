using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegisterAPI.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ 2. Add Controllers
builder.Services.AddControllers();

// ✅ 3. Add CORS for Netlify + Localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAndNetlify", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://appointment-web.netlify.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ 4. Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

var app = builder.Build();

// ✅ 5. Use Middlewares — correct order matters!
app.UseRouting();

app.UseCors("AllowLocalAndNetlify"); // ✅ must come before auth

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
