using System.Text;
using FluentValidation;
using Messegify.Application.Middleware;
using Messegify.Application.Services;
using Messegify.Application.Services.Configuration;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Infrastructure;
using Messegify.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Settings
var jwtConfig = configuration.GetRequiredSection("JwtConfiguration").Get<JwtConfiguration>();

// Add services to the container.
var services = builder.Services;

services.Configure<JwtConfiguration>(configuration.GetSection(nameof(JwtConfiguration)));

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true // TODO: maybe will create problems later
    };
});

services.AddDbContext<MessegifyDbContext>(contextOptionsBuilder =>
    contextOptionsBuilder.UseMySQL(configuration.GetConnectionString("MessegifyDatabaseConnectionString")));

services.AddValidatorsFromAssembly(typeof(Messegify.Domain.Validators.AssemblyMarker).Assembly);

services.AddScoped<IJwtService, JwtService>();

services.AddScoped<IRepository<Account>, Repository<Account, MessegifyDbContext>>();

services.AddScoped<IHashingService, HashingService>();
services.AddScoped<IAccountService, AccountService>();

services.AddScoped<ErrorHandlingMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

