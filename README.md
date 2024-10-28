# Descope ASP.NET Core Authentication Demo

This sample application demonstrates how to implement secure authentication in ASP.NET Core using the Descope .NET SDK. It provides examples of both cookie-based and Bearer token authentication for web applications and APIs.

## Features

- Login page with Descope's authentication web component
- Session management middleware supporting multiple authentication methods
- Protected routes using ASP.NET Core's `[Authorize]` attribute
- Examples for both Razor Pages and MVC Controllers

## Prerequisites

- .NET 6.0 or .NET 8.0
- A Descope account and Project ID

## Getting Started

1. Clone this repository
2. Update `appsettings.Development.json` with your Descope Project ID
3. Run the application in Visual Studio, or via the .NET CLI with `dotnet run`

## Implementation Details

The sample includes:

- Cookie-based authentication middleware for web applications
- Bearer token authentication for API scenarios
- Session validation and automatic token refresh
- User profile management examples

## Sample Code Structure

- `/Pages/Login.cshtml` - Login page with Descope web component
- `/Pages/Protected.cshtml` - Auth-protected Razor Page
- `/AuthenticationMiddleware.cs` - Cookie authentication middleware
- `/BearerAuthenticationMiddleware.cs` - Bearer Authentication middleware
- `/Controllers/ProtectedController` - Auth-protected API controller
- `/Controllers/RefreshController` - Token Refresh API controller

## Support

For SDK documentation and support, visit:
- [Descope .NET SDK Documentation](https://github.com/descope/descope-dotnet)
- [Descope Documentation](https://docs.descope.com/)