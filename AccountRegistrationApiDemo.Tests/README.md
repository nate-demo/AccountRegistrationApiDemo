# Integration Tests - AccountRegistrationApiDemo

This test project contains xUnit integration tests for the Account Registration API. These tests can be executed in VS Code and act like Postman tests, validating all API endpoints programmatically.

## 🚀 Running Tests

### Option 1: Run from VS Code (Recommended)

1. **Install Required Extensions**:
   - [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) - For running tests directly in VS Code

2. **Run Tests from Test Explorer**:
   - Open VS Code
   - Open the Test Explorer panel (Testing icon in the left sidebar, or Ctrl+Shift+T)
   - You'll see all test classes and individual tests listed
   - Click the ▶️ play button next to any test to run it
   - Click the ▶️ next to a class to run all tests in that class
   - Click the top ▶️ to run all tests

3. **Run Tests with Code Lens**:
   - Open any test file (e.g., `AccountsControllerTests.cs`)
   - You'll see "Run Test" and "Debug Test" links above each test method
   - Click "Run Test" to execute that specific test
   - Click "Debug Test" to debug with breakpoints

### Option 2: Run from Command Line

```bash
# Navigate to the solution directory
cd /path/to/AccountRegistrationApiDemo

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests in a specific test class
dotnet test --filter "FullyQualifiedName~AccountsControllerTests"

# Run a specific test
dotnet test --filter "FullyQualifiedName~GetAll_ReturnsOkResult_WithPaginatedAccounts"

# Run tests and generate code coverage report (requires additional packages)
dotnet test --collect:"XPlat Code Coverage"
```

### Option 3: Run from Terminal in VS Code

1. Open the integrated terminal in VS Code (Ctrl+`)
2. Navigate to the test project directory:
   ```bash
   cd AccountRegistrationApiDemo.Tests
   ```
3. Run tests:
   ```bash
   dotnet test
   ```

## 📁 Test Structure

```
AccountRegistrationApiDemo.Tests/
├── Controllers/
│   ├── AccountsControllerTests.cs    # Tests for /api/accounts endpoints
│   └── RegistrationsControllerTests.cs # Tests for /api/registrations endpoints
├── CustomWebApplicationFactory.cs   # Test server factory
├── IntegrationTestBase.cs           # Base class with shared test utilities
└── AccountRegistrationApiDemo.Tests.csproj
```

## 🧪 Test Coverage

### AccountsControllerTests
Tests for all Account endpoints:
- ✅ `GetAll_ReturnsOkResult_WithPaginatedAccounts` - GET /api/accounts
- ✅ `GetAll_WithPagination_ReturnsCorrectPageSize` - Pagination validation
- ✅ `GetById_WithValidId_ReturnsAccount` - GET /api/accounts/{id}
- ✅ `GetById_WithInvalidId_ReturnsNotFound` - 404 handling
- ✅ `Create_WithValidRequest_ReturnsCreatedAccount` - POST /api/accounts
- ✅ `Create_WithEmptyFields_ReturnsBadRequest` - Validation
- ✅ `Update_WithValidRequest_ReturnsUpdatedAccount` - PUT /api/accounts/{id}
- ✅ `Delete_WithValidId_ReturnsNoContent` - DELETE /api/accounts/{id}
- ✅ `CompleteWorkflow_CreateUpdateDeleteAccount_AllSucceed` - End-to-end workflow

### RegistrationsControllerTests
Tests for all Registration endpoints:
- ✅ `GetAll_ReturnsOkResult_WithPaginatedRegistrations` - GET /api/registrations
- ✅ `GetAll_WithPagination_ReturnsCorrectPageSize` - Pagination validation
- ✅ `GetAll_FilterByAccountId_ReturnsMatchingRegistrations` - Filter by account
- ✅ `GetAll_FilterByStatus_ReturnsMatchingRegistrations` - Filter by status
- ✅ `Create_WithValidRequest_ReturnsCreatedRegistration` - POST /api/registrations
- ✅ `Create_WithNonExistentAccountId_ReturnsBadRequest` - Validation
- ✅ `CompleteWorkflow_CreateAccountAndRegistrations_AllSucceed` - End-to-end workflow

## 🛠 Technology Stack

| Component | Package/Version |
|-----------|----------------|
| Test Framework | xUnit 2.9.3 |
| Test Host | Microsoft.AspNetCore.Mvc.Testing 10.0.3 |
| Assertions | FluentAssertions 8.8.0 |
| Test Runner | Microsoft.NET.Test.Sdk 17.14.1 |

## 🔍 How Tests Work

### Integration Tests
These tests use `WebApplicationFactory<Program>` to create an in-memory test server. This means:
- ✅ The entire ASP.NET Core pipeline is tested (controllers, middleware, services)
- ✅ No need to manually start the API - the test server starts automatically
- ✅ Tests are isolated - each test class gets its own server instance
- ✅ Fast execution - everything runs in memory
- ✅ Real HTTP requests - tests make actual HTTP calls to the in-memory server

### Test Base Class
All test classes inherit from `IntegrationTestBase` which provides:
- Pre-configured `HttpClient` for making requests
- JSON serialization options matching the API configuration (string enum conversion)
- Helper methods for deserializing responses

### Workflow Tests
Some tests validate complete user workflows:
1. Create an account
2. Create registrations for that account
3. Retrieve and verify data
4. Update account
5. Delete account
6. Verify cleanup

These ensure the API works correctly end-to-end, just like real user interactions.

## 📊 Viewing Test Results

### In VS Code
- Test Explorer shows pass/fail status with green ✓ or red ✗
- Click on a failed test to see the error message and stack trace
- Output panel shows detailed test execution logs

### Command Line
```bash
$ dotnet test

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    16, Skipped:     0, Total:    16, Duration: 747 ms
```

## 🐛 Debugging Tests

### In VS Code
1. Set breakpoints in test code or application code
2. Click "Debug Test" link above the test method
3. Test will pause at breakpoints
4. Use debug toolbar to step through code
5. Inspect variables in the Debug panel

### From Command Line
```bash
# Run with debugging support
dotnet test --logger "console;verbosity=detailed"
```

## 💡 Writing New Tests

### Example Test Structure
```csharp
[Fact]
public async Task YourTest_WithCondition_ExpectedOutcome()
{
    // Arrange - Set up test data and expectations
    var request = new SomeRequest { /* data */ };

    // Act - Execute the operation being tested
    var response = await Client.PostAsJsonAsync("/api/endpoint", request, JsonOptions);

    // Assert - Verify the outcome
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var result = await ReadFromJsonAsync<SomeResponse>(response.Content);
    result.Should().NotBeNull();
    result!.Property.Should().Be(expectedValue);
}
```

### Best Practices
- ✅ Use descriptive test names: `MethodName_Condition_ExpectedOutcome`
- ✅ Follow Arrange-Act-Assert pattern
- ✅ Test one thing per test
- ✅ Use FluentAssertions for readable assertions
- ✅ Clean up test data when needed (or let the in-memory store reset)
- ✅ Test both success and failure cases
- ✅ Test edge cases and validation

## 🔄 Continuous Integration

These tests can be integrated into CI/CD pipelines:

```yaml
# GitHub Actions example
- name: Run tests
  run: dotnet test --no-build --verbosity normal

# Azure DevOps example
- task: DotNetCoreCLI@2
  displayName: 'Run Integration Tests'
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
```

## 📝 Notes

- **Data Persistence**: Tests use the same in-memory data store as the application. Data created in tests may affect other tests running concurrently.
- **Test Isolation**: Each test class gets a fresh server instance, providing good isolation.
- **Performance**: Tests run quickly (typically 700-1000ms for all 16 tests).
- **No External Dependencies**: Tests don't require a database, external APIs, or any infrastructure.

## 🆚 Comparison: Integration Tests vs .http Files

| Feature | Integration Tests (xUnit) | .http Files (REST Client) |
|---------|---------------------------|---------------------------|
| **Automation** | Fully automated, runs in CI/CD | Manual execution |
| **Assertions** | Built-in with FluentAssertions | Manual validation |
| **Debugging** | Full debugging support | Limited |
| **Test Results** | Structured pass/fail reports | Visual response viewing |
| **Workflow Testing** | Easy multi-step workflows | Manual step-by-step |
| **Regression Detection** | Automatic failure detection | Manual review required |
| **Documentation** | Acts as executable documentation | Acts as API examples |
| **Best For** | Automated testing, CI/CD | Manual testing, exploration |

**Recommendation**: Use both!
- Use `.http` files for quick manual API exploration and debugging
- Use integration tests for automated regression testing and CI/CD

## 🎓 Learning Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [ASP.NET Core Integration Tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- [Testing in VS Code](https://code.visualstudio.com/docs/csharp/testing)

---

Happy Testing! 🎉
