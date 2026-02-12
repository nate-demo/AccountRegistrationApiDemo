using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.Models.Entities;

/// <summary>
/// Represents an event, course, or subscription registration linked to an account.
/// </summary>
public class Registration
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public RegistrationStatus Status { get; set; }
    public string EventOrCourseName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Details { get; set; } = string.Empty;
}
