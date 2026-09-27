using System.Text.Json;
using System.Text.Json.Serialization;

using AddressManagement.Application.Exceptions;
using AddressManagement.Application.Repositories;
using AddressManagement.Application.Services;
using AddressManagement.Application.Validators;
using AddressManagement.Infrastructure.Persistence;
using AddressManagement.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    options.UseNpgsql(connString)
        // The capped count in AddressRepository has no OrderBy on purpose.
        .ConfigureWarnings(w => w.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning)));

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

// Duplicate address (ConflictException) -> 409 with its message, anything else stays a generic 500.
builder.Services.AddProblemDetails(o => o.CustomizeProblemDetails = ctx =>
{
    if (ctx.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error is ConflictException conflict)
    {
        ctx.ProblemDetails.Detail = conflict.Message;
    }
});

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

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex is ConflictException ? StatusCodes.Status409Conflict : StatusCodes.Status500InternalServerError,
    SuppressDiagnosticsCallback = ctx => ctx.Exception is ConflictException // no error log for duplicates
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
