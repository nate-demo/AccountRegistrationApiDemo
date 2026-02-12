using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountRegistrationApiDemo.Tests;

/// <summary>
/// Base class for integration tests with JSON serialization helpers.
/// </summary>
public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory Factory;

    /// <summary>
    /// JSON serializer options that match the API configuration.
    /// </summary>
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    /// <summary>
    /// Reads and deserializes JSON content from HTTP response using API-compatible settings.
    /// </summary>
    protected static async Task<T?> ReadFromJsonAsync<T>(HttpContent content)
    {
        return await content.ReadFromJsonAsync<T>(JsonOptions);
    }
}
