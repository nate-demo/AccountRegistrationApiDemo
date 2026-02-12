using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.Models.Entities;

/// <summary>
/// Represents a user or organization account.
/// </summary>
public class Account
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
