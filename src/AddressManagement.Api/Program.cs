using System.Text.Json;
using System.Text.Json.Serialization;

using AddressManagement.Application;
using AddressManagement.Application.Repositories;
using AddressManagement.Application.Services;
using AddressManagement.Application.Validators;
using AddressManagement.Infrastructure.Persistence;
using AddressManagement.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// configuring for the auto-generation of the TS types from OpenAPI config
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseNpgsql(connString));

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRecipientRepository, RecipientRepository>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddValidatorsFromAssemblyContaining<AddressCreateDtoValidator>();

// Google Sign-In: the client sends Google's ID token as bearer token, validated against Google's public keys.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.Audience = builder.Configuration["Google:ClientId"];
        options.TokenValidationParameters.ValidIssuers = ["https://accounts.google.com", "accounts.google.com"];
    });
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()); // every endpoint

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
