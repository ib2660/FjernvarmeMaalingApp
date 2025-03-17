using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Factories;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Models.Strategies;
using FjernvarmeMaalingApp.ViewModels.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IUserRepository, UserRepositoryService>(); // Tilføj en implementering af IUserRepository.
builder.Services.AddAuthenticationCore(); // Tilføj autentificeringstjenester.
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationService>(); // Lad appens autentifikationsservice nedarve fra  AuthenticationStateProvider.
builder.Services.AddScoped<IAuthenticationService>(sp =>
    (AuthenticationService)sp.GetRequiredService<AuthenticationStateProvider>()); // Tilføj AuthenticationService som en implementering af IAuthenticationService, for at sikre enkel udskiftning af autentifikationsmåde.
builder.Services.AddScoped<IReadDataRepository, ReadDataRepository>(); // Tilføj en implementering af IReadDataRepository. Kontrakten beskriver kun læseadgang til datalaget. 
builder.Services.AddScoped<IWriteDataRepository, WriteDataRepository>(); // Tilføj en implementering af IWriteDataRepository. Kontrakten beskriver kun skriveadgang til datalaget. Dette sikrer kompatibilitet med CQRS mønster (command query responsibility segregation).

// Tilføj implementeringer af ITimeFrameStrategy.
builder.Services.AddScoped<ITimeFrameStrategy, YearlyTimeFrameStrategy>();
builder.Services.AddScoped<ITimeFrameStrategy, MonthlyTimeFrameStrategy>();
// Tilføj implementeringer af IConsumptionTypeFactory.
builder.Services.AddScoped<IConsumptionTypeFactory, OlieForbrugFactory>();
builder.Services.AddScoped<IConsumptionTypeFactory, GasForbrugFactory>();
builder.Services.AddScoped<IConsumptionTypeFactory, FjernvarmeForbrugFactory>();
// Tilføj implementeringer af IRegistrationStrategy.
builder.Services.AddScoped<IRegistrationStrategy , IntegerRegistrationStrategy>();
builder.Services.AddScoped<IRegistrationStrategy, MeterReadingRegistrationStrategy>();
// Tilføj implementeringer af IMeasurementDisplayStrategy.
builder.Services.AddScoped<IMeasurementDisplayStrategy, FullDisplayStrategy>();
builder.Services.AddScoped<IMeasurementDisplayStrategy, ListMonthsDisplayStrategy>();
builder.Services.AddScoped<IMeasurementDisplayStrategy, OneMonthDisplayStrategy>();
builder.Services.AddScoped<IMeasurementDisplayStrategy, SameMonthAllYearsDisplayStrategy>();
// Tilføj listen over services
builder.Services.AddScoped<IServicesRegistry, ServicesRegistry>();
// Tilføj scoped singletons af ViewModels.
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<OpretBrugerViewModel>();
builder.Services.AddScoped<AflæsDataViewModel>();
builder.Services.AddScoped<GemDataViewModel>();
builder.Services.AddScoped<BrugerOpsætningViewModel>();

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
