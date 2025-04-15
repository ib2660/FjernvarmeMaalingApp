using FjernvarmeMaalingApp.Data;
using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Models.Strategies;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Factories;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Strategies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using static FjernvarmeMaalingApp.Models.User;

namespace FjernvarmeMaalingApp.Configuration;
public static class ServiceRegistration
{
    // Tilføj en implementering af IUserRepository.
    // Tilføj en implementering af IReadDataRepository. Kontrakten beskriver kun læseadgang til datalaget. 
    // Tilføj en implementering af IWriteDataRepository. Kontrakten beskriver kun skriveadgang til datalaget. Dette sikrer kompatibilitet med CQRS mønster (command query responsibility segregation).
    public static void RegisterRepositories(IServiceCollection services)
    {
        _ = services.AddScoped<IUserRepository, UserRepositoryService>();
        _ = services.AddScoped<IReadDataRepository, ReadDataRepository>();
        _ = services.AddScoped<IWriteDataRepository, WriteDataRepository>();
    }

    // Tilføj autentificeringstjenester.
    // Lad appens autentifikationsservice nedarve fra  AuthenticationStateProvider.
    // Tilføj AuthenticationService som en implementering af IAuthenticationService, for at sikre enkel udskiftning af autentifikationsmåde.
    public static void RegisterAuthentication(IServiceCollection services)
    {
        _ = services.AddAuthenticationCore();
        _ = services.AddScoped<AuthenticationStateProvider, AuthenticationService>();
        services.AddScoped<IAuthenticationService>(sp =>
            (AuthenticationService)sp.GetRequiredService<AuthenticationStateProvider>());
    }

    // Tilføj implementeringer af ITimeFrameStrategy.
    // Tilføj implementeringer af IRegistrationStrategy.
    // Tilføj implementeringer af IMeasurementDisplayStrategy.
    public static void RegisterStrategies(IServiceCollection services)
    {
        _ = services.AddScoped<ITimeFrameStrategy, YearlyTimeFrameStrategy>();
        _ = services.AddScoped<ITimeFrameStrategy, MonthlyTimeFrameStrategy>();
        _ = services.AddScoped<IRegistrationStrategy, IntegerRegistrationStrategy>();
        _ = services.AddScoped<IRegistrationStrategy, MeterReadingRegistrationStrategy>();
        _ = services.AddScoped<IMeasurementDisplayStrategy, FullDisplayStrategy>();
        _ = services.AddScoped<IMeasurementDisplayStrategy, ListMonthsDisplayStrategy>();
        _ = services.AddScoped<IMeasurementDisplayStrategy, OneMonthDisplayStrategy>();
        _ = services.AddScoped<IMeasurementDisplayStrategy, SameMonthAllYearsDisplayStrategy>();
    }

    // Tilføj implementeringer af IConsumptionTypeFactory.
    // Tilføj listen over services
    public static void RegisterFactories(IServiceCollection services)
    {
        _ = services.AddScoped<IConsumptionTypeFactory, OlieForbrugFactory>();
        _ = services.AddScoped<IConsumptionTypeFactory, GasForbrugFactory>();
        _ = services.AddScoped<IConsumptionTypeFactory, FjernvarmeForbrugFactory>();
        _ = services.AddScoped<IServicesRegistry, ServicesRegistry>();
        _ = services.AddScoped<IUserFactory, UserFactory>();
    }

    // Tilføj scoped singletons af ViewModels.
    public static void RegisterViewModels(IServiceCollection services)
    {
        _ = services.AddScoped<LoginViewModel>();
        _ = services.AddScoped<OpretBrugerViewModel>();
        _ = services.AddScoped<AflæsDataViewModel>();
        _ = services.AddScoped<GemDataViewModel>();
        _ = services.AddScoped<BrugerOpsætningViewModel>();
    }

    public static void RegisterLogging(IServiceCollection services)
    {
        _ = services.AddLogging(config =>
        {
            _ = config.AddConsole();
            _ = config.AddDebug();
        });
    }
}