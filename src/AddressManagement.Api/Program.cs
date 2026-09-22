using AddressManagement.Application;
using AddressManagement.Application.Mappers;
using AddressManagement.Application.Services;
using AddressManagement.Domain;
using AddressManagement.Infrastructure.Persistence;
using AddressManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseNpgsql(connString));

builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AddressDbContext>().Database.MigrateAsync();
}

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
