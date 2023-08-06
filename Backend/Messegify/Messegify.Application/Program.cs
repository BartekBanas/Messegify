using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using Messegify.Application;
using Messegify.Application.Authorization;
using Messegify.Application.Authorization.Handlers;
using Messegify.Application.Authorization.Requirements;
using Messegify.Application.Configuration;
using Messegify.Application.Extensions;
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

services.AddCors();

services.AddScoped<IAuthorizationHandler, ChatRoomAuthorizationHandler>();
services.AddScoped<IAuthorizationHandler, AccountAuthorizationHandler>();

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
services.AddSwaggerGenWithAuthorization();
services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearerOptions =>
{
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});


services.AddDbContext<MessegifyDbContext>(contextOptionsBuilder =>
    {
        contextOptionsBuilder.UseMySQL(configuration.GetConnectionString("MessegifyDatabaseConnectionString") ?? throw new InvalidOperationException(),
            optionsBuilder => { optionsBuilder.MigrationsAssembly("Messegify.Application"); });
    }
);

services.AddValidatorsFromAssembly(typeof(Messegify.Domain.Validators.AssemblyMarker).Assembly);

services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

services.AddScoped<IJwtService, JwtService>();

services.AddScoped<IRepository<Account>,     Repository<Account,  MessegifyDbContext>>();
services.AddScoped<IRepository<User>,        Repository<User,     MessegifyDbContext>>();
services.AddScoped<IRepository<Contact>,     Repository<Contact,  MessegifyDbContext>>();
services.AddScoped<IRepository<ChatRoom>,    Repository<ChatRoom, MessegifyDbContext>>();
services.AddScoped<IRepository<Message>,     Repository<Message,  MessegifyDbContext>>();    

services.AddScoped<IHashingService, HashingService>();
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IChatRoomRequestHandler, ChatRoomRequestHandler>();
services.AddScoped<IMessageRequestHandler, MessageRequestHandler>();

services.AddMediatR(typeof(Messegify.Application.DomainEventHandlers.AssemblyMarker));

services.AddScoped<ErrorHandlingMiddleware>();
services.AddScoped<InfrastructureErrorHandlingMiddleware>();

var app = builder.Build();

// app.Services.CreateScope().ServiceProvider.GetRequiredService<MessegifyDbContext>().Database.EnsureDeleted();
app.Services.CreateScope().ServiceProvider.GetRequiredService<MessegifyDbContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .AllowAnyHeader()
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowCredentials());

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<InfrastructureErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();