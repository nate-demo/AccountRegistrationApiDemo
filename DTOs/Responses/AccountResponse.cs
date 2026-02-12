using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.DTOs.Responses;

/// <summary>
/// Account data returned by list and single-get endpoints.
/// </summary>
public class AccountResponse
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
}
