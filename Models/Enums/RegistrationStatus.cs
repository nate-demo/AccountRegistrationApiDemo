namespace AccountRegistrationApiDemo.Models.Enums;

/// <summary>
/// Represents the current status of a registration.
/// </summary>
public enum RegistrationStatus
{
    /// <summary>Registration is awaiting confirmation.</summary>
    Pending,

    /// <summary>Registration has been confirmed.</summary>
    Confirmed,

    /// <summary>Registration has been cancelled.</summary>
    Cancelled,

    /// <summary>Registration has been completed successfully.</summary>
    Completed
}
