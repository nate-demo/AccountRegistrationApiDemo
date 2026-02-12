using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace AccountRegistrationApiDemo.Tests;

/// <summary>
/// Custom WebApplicationFactory for integration tests.
/// This creates an in-memory test server for testing the API.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // The application already uses in-memory data store
            // No additional configuration needed for testing
        });

        builder.UseEnvironment("Testing");
    }

    /// <summary>
    /// Creates an HTTP client with JSON options matching the API configuration.
    /// </summary>
    public HttpClient CreateClientWithJsonOptions()
    {
        var client = CreateClient();
        // The client will use the same JSON settings as the server through content negotiation
        return client;
    }
}
