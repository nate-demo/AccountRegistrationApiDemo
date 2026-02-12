using System.Net;
using System.Net.Http.Json;
using AccountRegistrationApiDemo.DTOs.Requests;
using AccountRegistrationApiDemo.DTOs.Responses;
using AccountRegistrationApiDemo.Models.Enums;
using FluentAssertions;

namespace AccountRegistrationApiDemo.Tests.Controllers;

/// <summary>
/// Integration tests for AccountsController endpoints.
/// These tests act like Postman tests and can be run from VS Code.
/// </summary>
public class AccountsControllerTests : IntegrationTestBase
{
    public AccountsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithPaginatedAccounts()
    {
        // Act
        var response = await Client.GetAsync("/api/accounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<AccountResponse>>(response.Content);
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
        var response = await Client.GetAsync($"/api/accounts?page=1&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<PaginatedResponse<AccountResponse>>(response.Content);
        result.Should().NotBeNull();
        result!.PageSize.Should().Be(pageSize);
        result.Data.Count.Should().BeLessThanOrEqualTo(pageSize);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsAccount()
    {
        // Arrange - First get all accounts to get a valid ID
        var allAccountsResponse = await Client.GetAsync("/api/accounts");
        var allAccounts = await ReadFromJsonAsync<PaginatedResponse<AccountResponse>>(allAccountsResponse.Content);
        var accountId = allAccounts!.Data.First().Id;

        // Act
        var response = await Client.GetAsync($"/api/accounts/{accountId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<AccountDetailResponse>(response.Content);
        result.Should().NotBeNull();
        result!.Id.Should().Be(accountId);
        result.Registrations.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = Guid.Empty;

        // Act
        var response = await Client.GetAsync($"/api/accounts/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidRequest_ReturnsCreatedAccount()
    {
        // Arrange
        var request = new CreateAccountRequest
        {
            FirstName = "Integration",
            LastName = "Test",
            Email = "integration.test@example.com",
            Phone = "+1-555-INT-TEST",
            Address = "123 Integration Test St, Test City, TC 12345",
            Status = AccountStatus.Active
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/accounts", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var result = await ReadFromJsonAsync<AccountResponse>(response.Content);
        result.Should().NotBeNull();
        result!.FirstName.Should().Be(request.FirstName);
        result.LastName.Should().Be(request.LastName);
        result.Email.Should().Be(request.Email);
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Create_WithEmptyFields_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateAccountRequest
        {
            FirstName = "",
            LastName = "",
            Email = "invalid-email"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/accounts", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_WithValidRequest_ReturnsUpdatedAccount()
    {
        // Arrange - Create an account first
        var createRequest = new CreateAccountRequest
        {
            FirstName = "Original",
            LastName = "Name",
            Email = "original@example.com",
            Phone = "+1-555-ORIGINAL",
            Address = "123 Original St",
            Status = AccountStatus.Active
        };

        var createResponse = await Client.PostAsJsonAsync("/api/accounts", createRequest, JsonOptions);
        var createdAccount = await ReadFromJsonAsync<AccountResponse>(createResponse.Content);

        var updateRequest = new UpdateAccountRequest
        {
            FirstName = "Updated",
            LastName = "Name",
            Email = "updated@example.com",
            Phone = "+1-555-UPDATED",
            Address = "456 Updated St",
            Status = AccountStatus.Inactive
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/accounts/{createdAccount!.Id}", updateRequest, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await ReadFromJsonAsync<AccountResponse>(response.Content);
        result.Should().NotBeNull();
        result!.FirstName.Should().Be(updateRequest.FirstName);
        result.Email.Should().Be(updateRequest.Email);
        result.Status.Should().Be(updateRequest.Status);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContent()
    {
        // Arrange - Create an account first
        var createRequest = new CreateAccountRequest
        {
            FirstName = "ToDelete",
            LastName = "Account",
            Email = "todelete@example.com",
            Phone = "+1-555-DELETE",
            Address = "123 Delete St",
            Status = AccountStatus.Active
        };

        var createResponse = await Client.PostAsJsonAsync("/api/accounts", createRequest, JsonOptions);
        var createdAccount = await ReadFromJsonAsync<AccountResponse>(createResponse.Content);

        // Act
        var response = await Client.DeleteAsync($"/api/accounts/{createdAccount!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the account is deleted
        var getResponse = await Client.GetAsync($"/api/accounts/{createdAccount.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CompleteWorkflow_CreateUpdateDeleteAccount_AllSucceed()
    {
        // Step 1: Create Account
        var createRequest = new CreateAccountRequest
        {
            FirstName = "Workflow",
            LastName = "Test",
            Email = "workflow.test@example.com",
            Phone = "+1-555-WORKFLOW",
            Address = "100 Workflow Lane",
            Status = AccountStatus.Active
        };

        var createResponse = await Client.PostAsJsonAsync("/api/accounts", createRequest, JsonOptions);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdAccount = await ReadFromJsonAsync<AccountResponse>(createResponse.Content);
        createdAccount.Should().NotBeNull();

        // Step 2: Get Account by ID
        var getResponse = await Client.GetAsync($"/api/accounts/{createdAccount!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Step 3: Update Account
        var updateRequest = new UpdateAccountRequest
        {
            FirstName = "WorkflowUpdated",
            LastName = "TestUpdated",
            Email = "workflow.updated@example.com",
            Phone = "+1-555-WFLOW-UPD",
            Address = "200 Updated Lane",
            Status = AccountStatus.Inactive
        };

        var updateResponse = await Client.PutAsJsonAsync($"/api/accounts/{createdAccount.Id}", updateRequest, JsonOptions);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedAccount = await ReadFromJsonAsync<AccountResponse>(updateResponse.Content);
        updatedAccount!.FirstName.Should().Be(updateRequest.FirstName);

        // Step 4: Delete Account
        var deleteResponse = await Client.DeleteAsync($"/api/accounts/{createdAccount.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Step 5: Verify Account is Deleted
        var verifyResponse = await Client.GetAsync($"/api/accounts/{createdAccount.Id}");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
