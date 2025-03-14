using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Extensions;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Factories;
using Microsoft.Extensions.DependencyInjection;
using FjernvarmeMaalingApp.Services.Converters;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Models.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<IUserRepository, UserRepositoryService>(); // Tilf�j en implementering af IUserRepository.
builder.Services.AddAuthenticationCore(); // Tilf�j autentificeringstjenester.
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationService>(); // Lad appens autentifikationsservice nedarve fra  AuthenticationStateProvider.
builder.Services.AddScoped<IAuthenticationService>(sp =>
    (AuthenticationService)sp.GetRequiredService<AuthenticationStateProvider>()); // Tilf�j AuthenticationService som en implementering af IAuthenticationService, for at sikre enkel udskiftning af autentifikationsm�de.
builder.Services.AddScoped<IReadDataRepository, ReadDataRepository>(); // Tilf�j en implementering af IReadDataRepository. Kontrakten beskriver kun l�seadgang til datalaget. 
builder.Services.AddScoped<IWriteDataRepository, WriteDataRepository>(); // Tilf�j en implementering af IWriteDataRepository. Kontrakten beskriver kun skriveadgang til datalaget. Dette sikrer kompatibilitet med CQRS m�nster (command query responsibility segregation).

// Tilf�j implementeringer af IConsumptionTypeFactory.
builder.Services.AddTransient<IConsumptionTypeFactory, OlieForbrugFactory>();
builder.Services.AddTransient<IConsumptionTypeFactory, GasForbrugFactory>();
builder.Services.AddTransient<IConsumptionTypeFactory, FjernvarmeForbrugFactory>();
// Tilf�j implementeringer af IRegistrationStrategy.
builder.Services.AddTransient<IRegistrationStrategy , IntegerRegistrationStrategy>();
builder.Services.AddTransient<IRegistrationStrategy, MeterReadingRegistrationStrategy>();
// Tilf�j implementeringer af IMeasurementDisplayStrategy.
builder.Services.AddTransient<IMeasurementDisplayStrategy, FullDisplayStrategy>();
builder.Services.AddTransient<IMeasurementDisplayStrategy, ListMonthsDisplayStrategy>();
builder.Services.AddTransient<IMeasurementDisplayStrategy, OneMonthDisplayStrategy>();
builder.Services.AddTransient<IMeasurementDisplayStrategy, SameMonthAllYearsDisplayStrategy>();
// Tilf�j sacoped singletons af ViewModels.
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<OpretBrugerViewModel>();
builder.Services.AddScoped<Afl�sDataViewModel>();
builder.Services.AddScoped<GemDataViewModel>();
builder.Services.AddScoped<BrugerOps�tningViewModel>();

// Tilf�j JSON options.
builder.Services.AddCustomJsonOptions();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
