using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;

namespace AccountRegistrationApiDemo.Services;

/// <summary>
/// Defines operations for managing accounts.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Get a paginated, optionally filtered list of accounts.
    /// </summary>
    PaginatedResponse<AccountResponse> GetAll(int page, int pageSize, string? search);

    /// <summary>
    /// Get a single account by Id, including its registrations.
    /// </summary>
    AccountDetailResponse? GetById(Guid id);

    /// <summary>
    /// Create a new account.
    /// </summary>
    AccountResponse Create(CreateAccountRequest request);

    /// <summary>
    /// Update an existing account.
    /// </summary>
    AccountResponse? Update(Guid id, UpdateAccountRequest request);

    /// <summary>
    /// Delete an account and all of its registrations.
    /// </summary>
    bool Delete(Guid id);
}
