using Recommendations;
using Recommendations.Authorization.Api;
using Recommendations.Cart.Api;
using Recommendations.Dictionaries.Api;
using Recommendations.Purchase.Api;
using Recommendations.Shared.ModuleDefinition;

var builder = WebApplication.CreateBuilder(args);

RegistrationModules.RegisterModule<AuthorizationModule>();
RegistrationModules.RegisterModule<CartModule>();
RegistrationModules.RegisterModule<DictionariesModule>();
RegistrationModules.RegisterModule<PurchaseModule>();

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.UseApiDependencies();

app.Run();