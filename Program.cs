using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegisterAPI.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Configure Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ 2. Add Controllers
builder.Services.AddControllers();

// ✅ 3. Configure CORS for Netlify and Localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAndNetlify", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",                 // Local Angular/Vue/React
            "https://appointment-web.netlify.app"   // Netlify Deployment
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // Optional if you're using cookies or auth headers
    });
});

// ✅ 4. Configure JWT Authentication
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

// ✅ 5. Build the App
var app = builder.Build();

// ✅ 6. Configure Middleware (Order matters!)
app.UseRouting();

app.UseCors("AllowLocalAndNetlify"); // Must come BEFORE Authentication

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
