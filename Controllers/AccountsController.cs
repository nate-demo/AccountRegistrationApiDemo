using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Services;

namespace AccountRegistrationApiDemo.Controllers;

/// <summary>
/// Manages account CRUD operations and account-level registration queries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IRegistrationService _registrationService;
    private readonly IValidator<CreateAccountRequest> _createValidator;
    private readonly IValidator<UpdateAccountRequest> _updateValidator;

    public AccountsController(
        IAccountService accountService,
        IRegistrationService registrationService,
        IValidator<CreateAccountRequest> createValidator,
        IValidator<UpdateAccountRequest> updateValidator)
    {
        _accountService = accountService;
        _registrationService = registrationService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get a paginated list of accounts with optional search by name or email.
    /// </summary>
    /// <param name="page">Page number (1-based, default 1).</param>
    /// <param name="pageSize">Items per page (default 10, max 100).</param>
    /// <param name="search">Optional search term to filter by first name, last name, or email.</param>
    /// <returns>A paginated list of accounts.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<AccountResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = _accountService.GetAll(page, pageSize, search);
        return Ok(result);
    }

    /// <summary>
    /// Get a single account by Id, including its registrations.
    /// </summary>
    /// <param name="id">The account's unique identifier.</param>
    /// <returns>The account with its registrations, or 404 if not found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var result = _accountService.GetById(id);

        if (result is null)
            return NotFound(new { message = $"Account with Id '{id}' was not found." });

        return Ok(result);
    }

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <param name="request">The account creation payload.</param>
    /// <returns>The newly created account.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return ValidationProblem(new ValidationProblemDetails(
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));

        var created = _accountService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update an existing account (full replacement).
    /// </summary>
    /// <param name="id">The account's unique identifier.</param>
    /// <param name="request">The account update payload.</param>
    /// <returns>The updated account, or 404 if not found.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return ValidationProblem(new ValidationProblemDetails(
                validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())));

        var updated = _accountService.Update(id, request);

        if (updated is null)
            return NotFound(new { message = $"Account with Id '{id}' was not found." });

        return Ok(updated);
    }

    /// <summary>
    /// Delete an account and all of its registrations.
    /// </summary>
    /// <param name="id">The account's unique identifier.</param>
    /// <returns>204 No Content on success, or 404 if not found.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var deleted = _accountService.Delete(id);

        if (!deleted)
            return NotFound(new { message = $"Account with Id '{id}' was not found." });

        return NoContent();
    }

    /// <summary>
    /// Get all registrations for a specific account.
    /// </summary>
    /// <param name="id">The account's unique identifier.</param>
    /// <returns>A list of registrations belonging to the account.</returns>
    [HttpGet("{id:guid}/registrations")]
    [ProducesResponseType(typeof(List<RegistrationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetRegistrations(Guid id)
    {
        // Verify the account exists first
        var account = _accountService.GetById(id);
        if (account is null)
            return NotFound(new { message = $"Account with Id '{id}' was not found." });

        var registrations = _registrationService.GetByAccountId(id);
        return Ok(registrations);
    }
}
