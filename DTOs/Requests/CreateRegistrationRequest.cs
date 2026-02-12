using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.DTOs.Requests;

/// <summary>
/// Payload for creating a new registration linked to an account.
/// </summary>
public class CreateRegistrationRequest
{
    public Guid AccountId { get; set; }
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    public string EventOrCourseName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Details { get; set; } = string.Empty;
}
