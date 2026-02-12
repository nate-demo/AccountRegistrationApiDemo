using AutoMapper;
using AccountRegistrationApiDemo.Common.Extensions;
using AccountRegistrationApiDemo.Data;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Entities;
using AccountRegistrationApiDemo.Models.Enums;

namespace AccountRegistrationApiDemo.Services;

/// <summary>
/// In-memory implementation of <see cref="IRegistrationService"/>.
/// Reads from and writes to the singleton <see cref="InMemoryDataStore"/>.
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly InMemoryDataStore _store;
    private readonly IMapper _mapper;

    public RegistrationService(InMemoryDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public PaginatedResponse<RegistrationResponse> GetAll(int page, int pageSize, Guid? accountId, string? status)
    {
        var query = _store.Registrations.Values.AsEnumerable();

        // Optional filter by accountId
        if (accountId.HasValue)
        {
            query = query.Where(r => r.AccountId == accountId.Value);
        }

        // Optional filter by status string (case-insensitive)
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<RegistrationStatus>(status, ignoreCase: true, out var parsedStatus))
        {
            query = query.Where(r => r.Status == parsedStatus);
        }

        var sorted = query.OrderByDescending(r => r.RegistrationDate);
        var mapped = sorted.Select(r => _mapper.Map<RegistrationResponse>(r));

        return mapped.ToPaginatedResponse(page, pageSize);
    }

    /// <inheritdoc />
    public List<RegistrationResponse> GetByAccountId(Guid accountId)
    {
        return _store.Registrations.Values
            .Where(r => r.AccountId == accountId)
            .OrderByDescending(r => r.RegistrationDate)
            .Select(r => _mapper.Map<RegistrationResponse>(r))
            .ToList();
    }

    /// <inheritdoc />
    public RegistrationResponse? Create(CreateRegistrationRequest request)
    {
        // Verify the referenced account exists
        if (!_store.Accounts.ContainsKey(request.AccountId))
            return null;

        var registration = _mapper.Map<Registration>(request);
        registration.Id = Guid.NewGuid();
        registration.RegistrationDate = DateTime.UtcNow;

        _store.Registrations.TryAdd(registration.Id, registration);

        return _mapper.Map<RegistrationResponse>(registration);
    }
}
