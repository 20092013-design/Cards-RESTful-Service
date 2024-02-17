using Cards.Data;
using Cards.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDBContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DBConn")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<CardsService, CardsService>();
builder.Services.AddSingleton<SieveProcessor>();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cards API",
        Description = "This is an API for Cards, Users can Create cards. Users of role Member can access cards they created but admin can access all cards",
       
        Contact = new OpenApiContact
        {
            Name = "Naomi",
            Email = "yatornaomi34@gmail.com"
        }
    });
    
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["JWT.ValidAudience"],
            ValidIssuer = builder.Configuration["JWT.ValidIssuer"]
        };
    });


var app = builder.Build();

//builder.Services.AddSwaggerGen(c =>
//{

//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MOMO API", Version = "v1" });
//    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MOMO API");
        // Add Bearer token authorization
        c.OAuthClientId("swagger-ui");
        c.OAuthAppName("Swagger UI");
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
