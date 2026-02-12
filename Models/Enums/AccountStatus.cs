namespace AccountRegistrationApiDemo.Models.Enums;

/// <summary>
/// Represents the current status of an account.
/// </summary>
public enum AccountStatus
{
    /// <summary>Account is active and in good standing.</summary>
    Active,

    /// <summary>Account has been deactivated.</summary>
    Inactive,

    /// <summary>Account has been suspended due to policy violation or review.</summary>
    Suspended
}
