using FluentValidation;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.Validators;

/// <summary>
/// Validation rules for <see cref="CreateRegistrationRequest"/>.
/// </summary>
public class CreateRegistrationRequestValidator : AbstractValidator<CreateRegistrationRequest>
{
    public CreateRegistrationRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId is required.");

        RuleFor(x => x.EventOrCourseName)
            .NotEmpty().WithMessage("Event or course name is required.")
            .MaximumLength(200).WithMessage("Event or course name must not exceed 200 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be Pending, Confirmed, Cancelled, or Completed.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).When(x => x.Amount.HasValue)
            .WithMessage("Amount must be zero or a positive value.");

        RuleFor(x => x.Details)
            .MaximumLength(1000).WithMessage("Details must not exceed 1000 characters.");
    }
}
