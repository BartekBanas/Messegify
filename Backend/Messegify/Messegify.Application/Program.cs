using Messegify.Domain.Entities;
using Messegify.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<MessegifyDbContext>(contextOptionsBuilder =>
    contextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("MessegifyDatabaseConnectionString")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var dbcontext = app.Services.CreateScope().ServiceProvider.GetRequiredService<MessegifyDbContext>();
dbcontext.Accounts.Add(new Account()
{
    Email = "ne@se.pl",
    Name = "Noracism"
});

dbcontext.SaveChanges();

app.Run();

