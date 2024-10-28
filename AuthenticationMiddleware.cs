using Descope;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DescopeTestApp
{
	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly DescopeClient _authClient;

		public AuthenticationMiddleware(RequestDelegate next, DescopeClient descopeClient)
		{
			_next = next;
			_authClient = descopeClient;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string scheme = CookieAuthenticationDefaults.AuthenticationScheme;

			// Check if the endpoint requires authorization
			var endpoint = context.GetEndpoint();
			var requiresAuth = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() is not null;

			// Ensure the user is authenticated if the page requires it
			if (context.User.Identity?.IsAuthenticated == true && requiresAuth)
			{
				try
				{
					// Retrieve the JWT token from the user's claims
					var sessionToken = context.User.FindFirst("sessionToken")?.Value;
					var refreshToken = context.User.FindFirst("refreshToken")?.Value;

					if (sessionToken != null && refreshToken != null)
					{
						// Validate and refresh the session with Descope
						var sessionInfo = await _authClient.Auth.ValidateAndRefreshSession(sessionToken, refreshToken);
						var user = await _authClient.Auth.Me(refreshToken);

						// If the session is valid, update the claims
						Claim[] claims = [
							new Claim(ClaimTypes.Name, user.Name),
							new Claim(ClaimTypes.Email, user.Email),
							new Claim("jwtToken", sessionInfo.Jwt),
							new Claim("refreshToken", refreshToken),
						];

						var identity = new ClaimsIdentity(claims, scheme);
						var principal = new ClaimsPrincipal(identity);

						// Refresh the authentication cookie with updated tokens
						await context.SignInAsync(scheme, principal);
					}
					else
					{
						// No valid tokens found, sign the user out
						await context.SignOutAsync(scheme);
						context.Response.Redirect("/Login");
						return;
					}
				}
				catch (Exception ex)
				{
					// Session validation failed – log the user out
					Console.WriteLine($"Session validation failed: {ex.Message}");
					await context.SignOutAsync("Cookies");
					context.Response.Redirect("/Login");
					return;
				}
			}

			// Continue to the next middleware
			await _next(context);
		}
	}

}
