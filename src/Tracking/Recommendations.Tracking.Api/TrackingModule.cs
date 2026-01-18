using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendations.Dictionaries.Shared;
using Recommendations.Shared.ModuleDefinition;
using Recommendations.Tracking.Core;
using Recommendations.Tracking.Core.Repositories;
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

        app.MapGet("/cf/similar-items/{itemId}", async (
            [FromRoute] string itemId,
            [FromQuery] int? topCount,
            [FromServices] ICfEmbeddingRepository cfRepository,
            CancellationToken cancellationToken = default) =>
        {
            var count = topCount ?? 10;
            if (count <= 0 || count > 100)
                return Results.BadRequest(new { error = "topCount must be between 1 and 100" });

            try
            {
                var exists = await cfRepository.ItemEmbeddingExists(itemId, cancellationToken);
                if (!exists)
                    return Results.NotFound(new { error = $"No CF embedding found for item {itemId}" });

                var similarItems = await cfRepository.GetSimilarItems(itemId, count, cancellationToken);
                var items = similarItems.Select(x => new CfRecommendationDto(x.ItemId, x.SimilarityScore)).ToList();

                return Results.Ok(new CfRecommendationsResponse(items, items.Count, "cf-item-similarity"));
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get similar items: {ex.Message}");
            }
        });

        app.MapGet("/cf/similar-items/{itemId}/products", async (
            [FromRoute] string itemId,
            [FromQuery] int? topCount,
            [FromServices] ICfEmbeddingRepository cfRepository,
            [FromServices] IDictionariesModuleApi dictionariesApi,
            CancellationToken cancellationToken = default) =>
        {
            var count = topCount ?? 10;
            if (count <= 0 || count > 100)
                return Results.BadRequest(new { error = "topCount must be between 1 and 100" });

            try
            {
                var exists = await cfRepository.ItemEmbeddingExists(itemId, cancellationToken);
                if (!exists)
                    return Results.NotFound(new { error = $"No CF embedding found for item {itemId}" });

                var similarItems = await cfRepository.GetSimilarItems(itemId, count, cancellationToken);
                var productIds = similarItems
                    .Select(x => Guid.TryParse(x.ItemId, out var guid) ? guid : (Guid?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToArray();

                var products = await dictionariesApi.GetProductsByIdsForRecommendations(productIds);
                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get similar items: {ex.Message}");
            }
        });

        app.MapGet("/cf/for-user/{userKey}", async (
            [FromRoute] string userKey,
            [FromQuery] int? topCount,
            [FromServices] ICfEmbeddingRepository cfRepository,
            CancellationToken cancellationToken = default) =>
        {
            var count = topCount ?? 10;
            if (count <= 0 || count > 100)
                return Results.BadRequest(new { error = "topCount must be between 1 and 100" });

            try
            {
                var exists = await cfRepository.UserEmbeddingExists(userKey, cancellationToken);
                if (!exists)
                    return Results.NotFound(new { error = $"No CF embedding found for user {userKey}" });

                var recommendations = await cfRepository.GetRecommendationsForUser(userKey, count, cancellationToken);
                var items = recommendations.Select(x => new CfRecommendationDto(x.ItemId, x.SimilarityScore)).ToList();

                return Results.Ok(new CfRecommendationsResponse(items, items.Count, "cf-user-recommendation"));
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get recommendations: {ex.Message}");
            }
        });

        app.MapGet("/cf/for-user/{userKey}/products", async (
            [FromRoute] string userKey,
            [FromQuery] int? topCount,
            [FromServices] ICfEmbeddingRepository cfRepository,
            [FromServices] IDictionariesModuleApi dictionariesApi,
            CancellationToken cancellationToken = default) =>
        {
            var count = topCount ?? 10;
            if (count <= 0 || count > 100)
                return Results.BadRequest(new { error = "topCount must be between 1 and 100" });

            try
            {
                var exists = await cfRepository.UserEmbeddingExists(userKey, cancellationToken);
                if (!exists)
                    return Results.NotFound(new { error = $"No CF embedding found for user {userKey}" });

                var recommendations = await cfRepository.GetRecommendationsForUser(userKey, count, cancellationToken);
                var productIds = recommendations
                    .Select(x => Guid.TryParse(x.ItemId, out var guid) ? guid : (Guid?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToArray();

                var products = await dictionariesApi.GetProductsByIdsForRecommendations(productIds);
                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get recommendations for user: {ex.Message}");
            }
        });

        app.MapGet("/cf/stats", async (
            [FromServices] ICfEmbeddingRepository cfRepository,
            CancellationToken cancellationToken = default) =>
        {
            try
            {
                var userCount = await cfRepository.GetUserEmbeddingsCount(cancellationToken);
                var itemCount = await cfRepository.GetItemEmbeddingsCount(cancellationToken);

                return Results.Ok(new CfModelStatsResponse(userCount, itemCount));
            }
            catch (Exception ex)
            {
                return Results.Problem($"Failed to get CF stats: {ex.Message}");
            }
        });

        // Test endpoint
        app.MapGet("/test", () => Results.Ok(new { message = "Tracking API is working!" }));
    }
}