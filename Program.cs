using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shopsy_Project.BL;
using Shopsy_Project.DAL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

//Dependency Injection with scoped lifetime.
//For TestUser
builder.Services.AddScoped<IBL_TestUser, BL_TestUser>();
builder.Services.AddScoped<IDAL_TestUser, DAL_TestUser>();

////For Users
builder.Services.AddScoped<IBL_User, BL_User>();
builder.Services.AddScoped<IDAL_User, DAL_User>();

////For Products
builder.Services.AddScoped<IBL_Product, BL_Product>();
builder.Services.AddScoped<IDAL_Product, DAL_Product>();

////For Cart
builder.Services.AddScoped<IBL_Cart, BL_Cart>();
//builder.Services.AddScoped<IDAL_Cart, DAL_Cart_SP>();
builder.Services.AddScoped<IDAL_Cart, DAL_Cart>();

////For WishItem
builder.Services.AddScoped<IBL_WishItem, BL_WishItem>();
builder.Services.AddScoped<IDAL_WishItem, DAL_WishItem>();

builder.Services.AddScoped<IDAL_Auth, DAL_Auth>();
builder.Services.AddScoped<IBL_Auth, BL_Auth>();

builder.Services.AddScoped<IDAL_PurchaseOrder, DAL_PurchaseOrder>();
builder.Services.AddScoped<IBL_PurchaseOrder, BL_PurchaseOrder>();

builder.Services.AddScoped<IDAL_Brand, DAL_Brand>();
builder.Services.AddScoped<IBL_Brand, BL_Brand>();

builder.Services.AddScoped<IDAL_Category, DAL_Category>();
builder.Services.AddScoped<IBL_Category, BL_Category>();

builder.Services.AddScoped<IDAL_Feedback, DAL_Feedback>();
builder.Services.AddScoped<IBL_Feedback, BL_Feedback>();

builder.Services.AddControllers();

// Bind JWT settings
var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.ValidIssuer,
        ValidAudience = jwtSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();