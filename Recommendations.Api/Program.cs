using recommendations;
using Recommendations.Authorization.Api;
using Recommendations.Cart.Api;
using Recommendations.Shared.ModuleDefinition;

var builder = WebApplication.CreateBuilder(args);

RegistrationModules.RegisterModule<AuthorizationModule>();
RegistrationModules.RegisterModule<CartModule>();

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();
app.UseApiDependencies();

app.Run();