using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using Messegify.Application.Authorization;
using Messegify.Application.Authorization.Handlers;
using Messegify.Application.Authorization.Requirements;
using Messegify.Application.Configuration;
using Messegify.Application.Middleware;
using Messegify.Application.Services;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Infrastructure;
using Messegify.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Settings
var jwtConfig = configuration.GetRequiredSection("JwtConfiguration").Get<JwtConfiguration>();

// Add services to the container.
var services = builder.Services;


builder.Services.AddScoped<IAuthorizationHandler, ChatRoomAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AccountAuthorizationHandler>();

services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.IsMemberOf, policy =>
        policy.Requirements.Add(new IsMemberOfRequirement()));
    
    options.AddPolicy(AuthorizationPolicies.IsOwnerOf, policy =>
        policy.Requirements.Add(new IsOwnerRequirement()));
});


services.Configure<JwtConfiguration>(configuration.GetSection(nameof(JwtConfiguration)));

services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpContextAccessor();

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
services.AddScoped<IRepository<User>, Repository<User, MessegifyDbContext>>();
services.AddScoped<IRepository<Contact>, Repository<Contact, MessegifyDbContext>>();
services.AddScoped<IRepository<ChatRoom>, Repository<ChatRoom, MessegifyDbContext>>();

services.AddScoped<IHashingService, HashingService>();
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IChatRoomRequestHandler, ChatRoomRequestHandler>();


services.AddMediatR(typeof(Messegify.Application.DomainEventHandlers.AssemblyMarker));

services.AddScoped<ErrorHandlingMiddleware>();
var app = builder.Build();

// app.Services.CreateScope().ServiceProvider.GetRequiredService<MessegifyDbContext>().Database.EnsureDeleted();
// app.Services.CreateScope().ServiceProvider.GetRequiredService<MessegifyDbContext>().Database.EnsureCreated();

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

