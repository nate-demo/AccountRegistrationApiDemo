using AutoMapper;
using AccountRegistrationApiDemo.Common.Extensions;
using AccountRegistrationApiDemo.Data;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Entities;

namespace AccountRegistrationApiDemo.Services;

/// <summary>
/// In-memory implementation of <see cref="IAccountService"/>.
/// Reads from and writes to the singleton <see cref="InMemoryDataStore"/>.
/// </summary>
public class AccountService : IAccountService
{
    private readonly InMemoryDataStore _store;
    private readonly IMapper _mapper;

    public AccountService(InMemoryDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public PaginatedResponse<AccountResponse> GetAll(int page, int pageSize, string? search)
    {
        var query = _store.Accounts.Values.AsEnumerable();

        // Optional search by first name, last name, or email (case-insensitive)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(a =>
                a.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                a.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                a.Email.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var sorted = query.OrderBy(a => a.LastName).ThenBy(a => a.FirstName);
        var mapped = sorted.Select(a => _mapper.Map<AccountResponse>(a));

        return mapped.ToPaginatedResponse(page, pageSize);
    }

    /// <inheritdoc />
    public AccountDetailResponse? GetById(Guid id)
    {
        if (!_store.Accounts.TryGetValue(id, out var account))
            return null;

        var response = _mapper.Map<AccountDetailResponse>(account);

        // Include related registrations
        var registrations = _store.Registrations.Values
            .Where(r => r.AccountId == id)
            .OrderByDescending(r => r.RegistrationDate)
            .Select(r => _mapper.Map<RegistrationResponse>(r))
            .ToList();

        response.Registrations = registrations;

        return response;
    }

    /// <inheritdoc />
    public AccountResponse Create(CreateAccountRequest request)
    {
        var account = _mapper.Map<Account>(request);
        account.Id = Guid.NewGuid();
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;

        _store.Accounts.TryAdd(account.Id, account);

        return _mapper.Map<AccountResponse>(account);
    }

    /// <inheritdoc />
    public AccountResponse? Update(Guid id, UpdateAccountRequest request)
    {
        if (!_store.Accounts.TryGetValue(id, out var existing))
            return null;

        // Map updated fields onto the existing entity
        _mapper.Map(request, existing);
        existing.UpdatedAt = DateTime.UtcNow;

        // ConcurrentDictionary stores the reference, so the update is already visible
        return _mapper.Map<AccountResponse>(existing);
    }

    /// <inheritdoc />
    public bool Delete(Guid id)
    {
        if (!_store.Accounts.TryRemove(id, out _))
            return false;

        // Cascade-remove all registrations for this account
        var registrationIds = _store.Registrations.Values
            .Where(r => r.AccountId == id)
            .Select(r => r.Id)
            .ToList();

        foreach (var regId in registrationIds)
        {
            _store.Registrations.TryRemove(regId, out _);
        }

        return true;
    }
}
