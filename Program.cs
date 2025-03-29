using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

ServiceRegistration.RegisterRepositories(builder.Services);
ServiceRegistration.RegisterAuthentication(builder.Services);
ServiceRegistration.RegisterStrategies(builder.Services);
ServiceRegistration.RegisterFactories(builder.Services);
ServiceRegistration.RegisterViewModels(builder.Services);
ServiceRegistration.RegisterLogging(builder.Services);


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
