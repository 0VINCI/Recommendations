using Microsoft.EntityFrameworkCore;
using Recommendations;
using Recommendations.Authorization.Api;
using Recommendations.Authorization.Infrastructure.DAL;
using Recommendations.Cart.Api;
using Recommendations.Cart.Infrastructure.DAL;
using Recommendations.ContentBased.Api;
using Recommendations.ContentBased.Core.Data;
using Recommendations.Dictionaries.Api;
using Recommendations.Dictionaries.Infrastructure.DAL;
using Recommendations.Purchase.Api;
using Recommendations.Purchase.Core.Data;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.Tracking.Api;
using Recommendations.Tracking.Core.Data.Tracking;
using Recommendations.Tracking.Core.Data.Signals;

var builder = WebApplication.CreateBuilder(args);

RegistrationModules.RegisterModule<AuthorizationModule>();
RegistrationModules.RegisterModule<CartModule>();
RegistrationModules.RegisterModule<ContentBasedModule>();
RegistrationModules.RegisterModule<DictionariesModule>();
RegistrationModules.RegisterModule<PurchaseModule>();
RegistrationModules.RegisterModule<TrackingModule>();

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.UseApiDependencies();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

var contexts = new DbContext[]
{
    scope.ServiceProvider.GetRequiredService<DictionariesDbContext>(),
    scope.ServiceProvider.GetRequiredService<PurchaseDbContext>(),
    scope.ServiceProvider.GetRequiredService<CartDbContext>(),
    scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>(),
    scope.ServiceProvider.GetRequiredService<ContentBasedDbContext>(),
    scope.ServiceProvider.GetRequiredService<TrackingDbContext>(),
    scope.ServiceProvider.GetRequiredService<SignalsDbContext>()
};

foreach (var ctx in contexts)
{
    try
    {
        await ctx.Database.MigrateAsync();
        logger.LogInformation("Applied migrations for {Context}", ctx.GetType().Name);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Migration failed for {Context}", ctx.GetType().Name);
        throw;
    }
}

app.Run();