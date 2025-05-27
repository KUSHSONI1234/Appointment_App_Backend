using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegisterAPI.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Add Controllers
builder.Services.AddControllers();

// ✅ Add JWT Auth
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

// ✅ Add CORS for both localhost and Netlify
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

var app = builder.Build(); // ✅ app created here

// ✅ Middleware — after `app` is created
app.UseCors("AllowLocalAndNetlify");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
