using System.Collections.Concurrent;
using AccountRegistrationApiDemo.Models.Entities;

namespace AccountRegistrationApiDemo.Data;

/// <summary>
/// Thread-safe in-memory data store that holds all accounts and registrations.
/// Registered as a singleton — data lives for the lifetime of the application.
/// </summary>
public class InMemoryDataStore
{
    /// <summary>
    /// All accounts keyed by their unique Id.
    /// </summary>
    public ConcurrentDictionary<Guid, Account> Accounts { get; } = new();

    /// <summary>
    /// All registrations keyed by their unique Id.
    /// </summary>
    public ConcurrentDictionary<Guid, Registration> Registrations { get; } = new();
}
