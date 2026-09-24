using System.Globalization;

using AddressManagement.Application;
using AddressManagement.Application.Services;
using AddressManagement.Application.Validators;
using AddressManagement.Infrastructure.Persistence;
using AddressManagement.Infrastructure.Repositories;
using FluentValidation;
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
builder.Services.AddValidatorsFromAssemblyContaining<AddressCreateDtoValidator>();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("de");

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AddressDbContext>();
    await db.Database.MigrateAsync();

    // Opt-in: dotnet run -- SeedAddresses=100000
    var seedCount = app.Configuration.GetValue<int?>("SeedAddresses");
    if (seedCount is > 0)
    {
        await DevDataSeeder.Seed(db, seedCount.Value);
    }
}

app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();

app.Run();
