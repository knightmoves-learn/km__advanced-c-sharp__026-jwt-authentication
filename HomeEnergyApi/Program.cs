using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoMapper;
using HomeEnergyApi.Models;
using HomeEnergyApi.Services;
using HomeEnergyApi.Dtos;
using HomeEnergyApi.Filters;
using HomeEnergyApi.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HomeRepository>();
builder.Services.AddScoped<IReadRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());
builder.Services.AddScoped<IWriteRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());
builder.Services.AddScoped<IOwnerLastNameQueryable<Home>>(provider => provider.GetRequiredService<HomeRepository>());

builder.Services.AddScoped<UtilityProviderRepository>();
builder.Services.AddScoped<IReadRepository<int, UtilityProvider>>(provider => provider.GetRequiredService<UtilityProviderRepository>());
builder.Services.AddScoped<IWriteRepository<int, UtilityProvider>>(provider => provider.GetRequiredService<UtilityProviderRepository>());

builder.Services.AddScoped<HomeUtilityProviderRepository>();
builder.Services.AddScoped<IReadRepository<int, HomeUtilityProvider>>(provider => provider.GetRequiredService<HomeUtilityProviderRepository>());
builder.Services.AddScoped<IWriteRepository<int, HomeUtilityProvider>>(provider => provider.GetRequiredService<HomeUtilityProviderRepository>());

builder.Services.AddTransient<ZipCodeLocationService>();
builder.Services.AddHttpClient<ZipCodeLocationService>();

builder.Services.AddTransient<HomeUtilityProviderService>();

builder.Services.AddDbContext<HomeDbContext>(options =>
    options.UseSqlite("Data Source=Homes.db").ConfigureWarnings(warings =>
    warings.Ignore(RelationalEventId.NonTransactionalMigrationOperationWarning)));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddAutoMapper(typeof(HomeProfile));

builder.Configuration.AddJsonFile("secrets.json");
        
builder.Services.AddAuthentication("JwtAuthentication")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("JwtAuthentication", null);


builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HomeDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

//Do NOT remove anything below this comment, this is required to autograde the lesson
public partial class Program { }