using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.Tracking.Core;
using Recommendations.Tracking.Core.Types;
using Recommendations.Tracking.Shared;
using Recommendations.Tracking.Shared.DTO;

namespace Recommendations.Tracking.Api;

public class TrackingModule : ModuleDefinition
{
    public override string ModulePrefix => "/tracking";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
    }

    public override void CreateEndpoints(IEndpointRouteBuilder app)
    {
        // Track event endpoint (frontend uses /track)
        app.MapPost("/track", async (
            [FromBody] TrackEventRequest request,
            [FromServices] ITrackingModuleApi trackingApi,
            CancellationToken cancellationToken = default) =>
        {
            if (string.IsNullOrEmpty(request.UserId) && !request.AnonymousId.HasValue)
            {
                return Results.BadRequest(new { error = "Either UserId or AnonymousId must be provided" });
            }

            try
            {
                var contextJson = request.Context != null ? JsonSerializer.Serialize(request.Context) : null;
                var payloadJson = request.Payload != null ? JsonSerializer.Serialize(request.Payload) : null;

                var eventId = await trackingApi.TrackEventAsync(
                    request.EventType,
                    request.Source,
                    request.UserId,
                    request.AnonymousId,
                    request.SessionId,
                    contextJson,
                    payloadJson,
                    cancellationToken);

                return Results.Ok(new { eventId });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to track event: {ex.Message}");
            }
        });
        
        // Legacy endpoint for backwards compatibility
        app.MapPost("/events", async (
            [FromBody] TrackEventRequest request,
            [FromServices] ITrackingModuleApi trackingApi,
            CancellationToken cancellationToken = default) =>
        {
            if (string.IsNullOrEmpty(request.UserId) && !request.AnonymousId.HasValue)
            {
                return Results.BadRequest(new { error = "Either UserId or AnonymousId must be provided" });
            }

            try
            {
                var contextJson = request.Context != null ? JsonSerializer.Serialize(request.Context) : null;
                var payloadJson = request.Payload != null ? JsonSerializer.Serialize(request.Payload) : null;

                var eventId = await trackingApi.TrackEventAsync(
                    request.EventType,
                    request.Source,
                    request.UserId,
                    request.AnonymousId,
                    request.SessionId,
                    contextJson,
                    payloadJson,
                    cancellationToken);

                return Results.Ok(new { eventId });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to track event: {ex.Message}");
            }
        });

        // Link identity endpoint (frontend uses /link-identity)
        app.MapPost("/link-identity", async (
            [FromBody] LinkIdentityRequest request,
            [FromServices] ITrackingModuleApi trackingApi,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await trackingApi.LinkIdentityAsync(request.AnonymousId, request.UserId, cancellationToken);
                return Results.Ok(new { message = "Identity linked successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to link identity: {ex.Message}");
            }
        });
        
        // Legacy endpoint for backwards compatibility
        app.MapPost("/identity/link", async (
            [FromBody] LinkIdentityRequest request,
            [FromServices] ITrackingModuleApi trackingApi,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                await trackingApi.LinkIdentityAsync(request.AnonymousId, request.UserId, cancellationToken);
                return Results.Ok(new { message = "Identity linked successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to link identity: {ex.Message}");
            }
        });

        app.MapGet("/identity/exists", async (
            [FromQuery] Guid anonymousId,
            [FromQuery] string userId,
            [FromServices] ITrackingModuleApi trackingApi,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var exists = await trackingApi.IdentityLinkExistsAsync(anonymousId, userId, cancellationToken);
                return Results.Ok(new { exists });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to check identity link: {ex.Message}");
            }
        });

        // Test endpoint
        app.MapGet("/test", () => Results.Ok(new { message = "Tracking API is working!" }));
    }
}