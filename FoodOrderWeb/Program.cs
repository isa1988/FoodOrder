using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Data;
using FoodOrderWeb.DAL.Data.Contracts;
using FoodOrderWeb.DAL.Repositories;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.DAL.Unit;
using FoodOrderWeb.Service.Services;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Starting
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
optionsBuilder.UseSqlServer(connection);
builder.Services.AddSingleton<IDataDbContextFactory>(
    sp => new DataDbContextFactory(optionsBuilder.Options));

//Add Identity & JWT authentication
//Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataDbContext>()
    .AddSignInManager()
    .AddRoles<Role>();

// JWT 
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
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

//Add authentication to Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

//Ending...
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
