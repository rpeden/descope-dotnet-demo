using Descope;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Security.Claims;

namespace DescopeTestApp
{
    public class BearerAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DescopeClient _authClient;

        public BearerAuthenticationMiddleware(RequestDelegate next, DescopeClient descopeClient)
        {
            _next = next;
            _authClient = descopeClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is targeting an MVC Controller with [Authorize]
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() is null)
            {
                // Not an authorized endpoint; skip middleware
                await _next(context);
                return;
            }

            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor is null)
            {
                // Not an MVC controller; skip middleware
                await _next(context);
                return;
            }

            // Extract the Bearer token from the Authorization header
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) ||
                !authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Bearer token is missing.");
                return;
            }

            var bearerToken = authHeader.ToString().Substring("Bearer ".Length).Trim();

            try
            {
                // Validate the Bearer token using Descope SDK
                var token = await _authClient.Auth.ValidateSession(bearerToken);
                if (token is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid bearer token.");
                    return;
                }

                // Proceed to the next middleware
                await _next(context);
            }
            catch (DescopeException ex)
            {
                // Token validation failed
                Console.WriteLine($"Bearer token validation failed: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Token validation failed.");
            }
        }
    }
}
