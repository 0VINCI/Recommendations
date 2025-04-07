using recommendations;
using Recommendations.Authorization;
using Recommendations.Shared;

var builder = WebApplication.CreateBuilder(args);

Modules.RegisterModule<AuthorizationModule>();

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();
app.UseApiDependencies();

app.Run();