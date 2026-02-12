using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.DTOs.Requests;

/// <summary>
/// Payload for updating an existing account.
/// </summary>
public class UpdateAccountRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public AccountStatus Status { get; set; }
}
