using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.DTOs.Responses;

/// <summary>
/// Registration data returned by API endpoints.
/// </summary>
public class RegistrationResponse
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public RegistrationStatus Status { get; set; }
    public string EventOrCourseName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Details { get; set; } = string.Empty;
}
