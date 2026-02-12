using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.DTOs.Responses;

/// <summary>
/// Detailed account view that includes the account's registrations.
/// Returned by GET /api/accounts/{id}.
/// </summary>
public class AccountDetailResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public AccountStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// All registrations belonging to this account.
    /// </summary>
    public List<RegistrationResponse> Registrations { get; set; } = [];
}
