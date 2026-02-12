using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Services;

namespace AccountRegistrationApiDemo.Controllers;

/// <summary>
/// Manages registration operations across all accounts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly IValidator<CreateRegistrationRequest> _createValidator;

    public RegistrationsController(
        IRegistrationService registrationService,
        IValidator<CreateRegistrationRequest> createValidator)
    {
        _registrationService = registrationService;
        _createValidator = createValidator;
    }

    /// <summary>
    /// Get a paginated list of all registrations with optional filters.
    /// </summary>
    /// <param name="page">Page number (1-based, default 1).</param>
    /// <param name="pageSize">Items per page (default 10, max 100).</param>
    /// <param name="accountId">Optional filter by account Id.</param>
    /// <param name="status">Optional filter by status (Pending, Confirmed, Cancelled, Completed).</param>
    /// <returns>A paginated list of registrations.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<RegistrationResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? accountId = null,
        [FromQuery] string? status = null)
    {
        var result = _registrationService.GetAll(page, pageSize, accountId, status);
        return Ok(result);
    }

    /// <summary>
    /// Create a new registration for an existing account.
    /// </summary>
    /// <param name="request">The registration creation payload.</param>
    /// <returns>The newly created registration, or 400 if the account does not exist.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRegistrationRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return ValidationProblem(new ValidationProblemDetails(
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));

        var created = _registrationService.Create(request);

        if (created is null)
            return BadRequest(new { message = $"Account with Id '{request.AccountId}' does not exist." });

        return CreatedAtAction(null, new { id = created.Id }, created);
    }
}
