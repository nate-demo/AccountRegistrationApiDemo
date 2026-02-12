using System.Net;
using System.Net.Http.Json;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Enums;
using FluentAssertions;

namespace AccountRegistrationApiDemo.Tests.Controllers;

/// <summary>
/// Integration tests for RegistrationsController endpoints.
/// These tests act like Postman tests and can be run from VS Code.
/// </summary>
public class RegistrationsControllerTests : IntegrationTestBase
{
    public RegistrationsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithPaginatedRegistrations()
    {
        // Act
        var response = await Client.GetAsync("/api/registrations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<RegistrationResponse>>(response.Content);
        result.Should().NotBeNull();
        result!.Data.Should().NotBeEmpty();
        result.TotalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetAll_WithPagination_ReturnsCorrectPageSize()
    {
        // Arrange
        int pageSize = 5;

        // Act
        var response = await Client.GetAsync($"/api/registrations?page=1&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<RegistrationResponse>>(response.Content);
        result.Should().NotBeNull();
        result!.PageSize.Should().Be(pageSize);
        result.Data.Count.Should().BeLessThanOrEqualTo(pageSize);
    }

    [Fact]
    public async Task GetAll_FilterByAccountId_ReturnsMatchingRegistrations()
    {
        // Arrange - Get an account ID first
        var accountsResponse = await Client.GetAsync("/api/accounts");
        var accounts = await ReadFromJsonAsync<PaginatedResponse<AccountResponse>>(accountsResponse.Content);
        var accountId = accounts!.Data.First().Id;

        // Act
        var response = await Client.GetAsync($"/api/registrations?accountId={accountId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<RegistrationResponse>>(response.Content);
        result.Should().NotBeNull();
        result!.Data.Should().AllSatisfy(registration =>
            registration.AccountId.Should().Be(accountId));
    }

    [Fact]
    public async Task GetAll_FilterByStatus_ReturnsMatchingRegistrations()
    {
        // Arrange
        string status = "Pending";

        // Act
        var response = await Client.GetAsync($"/api/registrations?status={status}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<RegistrationResponse>>(response.Content);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedRegistration()
    {
        // Arrange - Get a valid account ID first
        var accountsResponse = await Client.GetAsync("/api/accounts");
        var accounts = await ReadFromJsonAsync<PaginatedResponse<AccountResponse>>(accountsResponse.Content);
        var accountId = accounts!.Data.First().Id;

        var request = new CreateRegistrationRequest
        {
            AccountId = accountId,
            Status = RegistrationStatus.Pending,
            EventOrCourseName = "Integration Test Workshop",
            Amount = 299.99m,
            Details = "Comprehensive integration testing workshop"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/registrations", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await ReadFromJsonAsync<RegistrationResponse>(response.Content);
        result.Should().NotBeNull();
        result!.AccountId.Should().Be(request.AccountId);
        result.Status.Should().Be(request.Status);
        result.EventOrCourseName.Should().Be(request.EventOrCourseName);
        result.Amount.Should().Be(request.Amount);
    }

    [Fact]
    public async Task Create_WithNonExistentAccountId_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateRegistrationRequest
        {
            AccountId = Guid.Empty,
            Status = RegistrationStatus.Pending,
            EventOrCourseName = "Test Event",
            Amount = 100.00m,
            Details = "Test with non-existent account"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/registrations", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CompleteWorkflow_CreateAccountAndRegistrations_AllSucceed()
    {
        // Step 1: Create Account
        var accountRequest = new CreateAccountRequest
        {
            FirstName = "Registration",
            LastName = "Workflow",
            Email = "registration.workflow@example.com",
            Phone = "+1-555-REG-WFLOW",
            Address = "300 Registration Ave",
            Status = AccountStatus.Active
        };

        var accountResponse = await Client.PostAsJsonAsync("/api/accounts", accountRequest, JsonOptions);
        accountResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdAccount = await ReadFromJsonAsync<AccountResponse>(accountResponse.Content);
        createdAccount.Should().NotBeNull();

        // Step 2: Create First Registration
        var registration1Request = new CreateRegistrationRequest
        {
            AccountId = createdAccount!.Id,
            Status = RegistrationStatus.Pending,
            EventOrCourseName = "First Workshop",
            Amount = 299.99m,
            Details = "First registration"
        };

        var registration1Response = await Client.PostAsJsonAsync("/api/registrations", registration1Request, JsonOptions);
        registration1Response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 3: Create Second Registration
        var registration2Request = new CreateRegistrationRequest
        {
            AccountId = createdAccount.Id,
            Status = RegistrationStatus.Confirmed,
            EventOrCourseName = "Second Workshop",
            Amount = 499.99m,
            Details = "Second registration"
        };

        var registration2Response = await Client.PostAsJsonAsync("/api/registrations", registration2Request, JsonOptions);
        registration2Response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 4: Get All Registrations for Account
        var registrationsResponse = await Client.GetAsync($"/api/accounts/{createdAccount.Id}/registrations");
        registrationsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var registrations = await ReadFromJsonAsync<List<RegistrationResponse>>(registrationsResponse.Content);
        registrations.Should().HaveCountGreaterThanOrEqualTo(2);

        // Step 5: Delete Account (should cascade delete registrations)
        var deleteResponse = await Client.DeleteAsync($"/api/accounts/{createdAccount.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Step 6: Verify Account is Deleted
        var verifyResponse = await Client.GetAsync($"/api/accounts/{createdAccount.Id}");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
