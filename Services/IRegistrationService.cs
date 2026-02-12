using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;

namespace AccountRegistrationApiDemo.Services;

/// <summary>
/// Defines operations for managing registrations.
/// </summary>
public interface IRegistrationService
{
    /// <summary>
    /// Get a paginated list of all registrations, with optional filters.
    /// </summary>
    PaginatedResponse<RegistrationResponse> GetAll(int page, int pageSize, Guid? accountId, string? status);

    /// <summary>
    /// Get all registrations for a specific account.
    /// </summary>
    List<RegistrationResponse> GetByAccountId(Guid accountId);

    /// <summary>
    /// Create a new registration for an existing account.
    /// </summary>
    RegistrationResponse? Create(CreateRegistrationRequest request);
}
