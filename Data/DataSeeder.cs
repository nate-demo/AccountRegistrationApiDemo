using System.Text.Json;
using System.Text.Json.Serialization;
using AccountRegistrationApiDemo.Models.Entities;

namespace AccountRegistrationApiDemo.Data;

/// <summary>
/// Reads the static JSON files and populates the <see cref="InMemoryDataStore"/>.
/// Called once at application startup.
/// </summary>
public static class DataSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Load accounts.json and registrations.json into the provided data store.
    /// </summary>
    public static async Task SeedAsync(InMemoryDataStore store, string contentRootPath)
    {
        var accountsPath = Path.Combine(contentRootPath, "Data", "accounts.json");
        var registrationsPath = Path.Combine(contentRootPath, "Data", "registrations.json");

        if (File.Exists(accountsPath))
        {
            var json = await File.ReadAllTextAsync(accountsPath);
            var accounts = JsonSerializer.Deserialize<List<Account>>(json, JsonOptions) ?? [];

            foreach (var account in accounts)
            {
                store.Accounts.TryAdd(account.Id, account);
            }
        }

        if (File.Exists(registrationsPath))
        {
            var json = await File.ReadAllTextAsync(registrationsPath);
            var registrations = JsonSerializer.Deserialize<List<Registration>>(json, JsonOptions) ?? [];

            foreach (var registration in registrations)
            {
                store.Registrations.TryAdd(registration.Id, registration);
            }
        }
    }
}
