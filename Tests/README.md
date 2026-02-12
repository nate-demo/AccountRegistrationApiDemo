# API Tests for Account Registration API Demo

This directory contains API tests that can be executed directly in VS Code, acting like Postman tests. These tests allow you to validate all API endpoints without leaving your code editor.

## 🚀 Quick Start

### Prerequisites

1. **Install the REST Client Extension for VS Code**
   - Open VS Code
   - Go to Extensions (Ctrl+Shift+X or Cmd+Shift+X on Mac)
   - Search for "REST Client" by Huachao Mao
   - Click Install
   - Or visit: https://marketplace.visualstudio.com/items?itemName=humao.rest-client

2. **Start the API**
   ```bash
   cd /path/to/AccountRegistrationApiDemo
   dotnet run
   ```
   The API should be running on `http://localhost:5000`

### Running Tests

1. Open any `.http` file in VS Code:
   - `AccountsController.http` - Tests for account endpoints
   - `RegistrationsController.http` - Tests for registration endpoints

2. You'll see "Send Request" links above each HTTP request

3. Click "Send Request" to execute the test

4. View the response in a split pane to the right

## 📁 Test Files

### AccountsController.http

Tests all account-related endpoints:
- **GET** `/api/accounts` - List all accounts (with pagination and search)
- **GET** `/api/accounts/{id}` - Get account by ID
- **POST** `/api/accounts` - Create a new account
- **PUT** `/api/accounts/{id}` - Update an account
- **DELETE** `/api/accounts/{id}` - Delete an account
- **GET** `/api/accounts/{id}/registrations` - Get account registrations

Includes validation tests for:
- Missing required fields
- Invalid email formats
- Non-existent account IDs
- 404 error handling

### RegistrationsController.http

Tests all registration-related endpoints:
- **GET** `/api/registrations` - List all registrations (with pagination and filters)
- **POST** `/api/registrations` - Create a new registration

Includes:
- Filtering by account ID
- Filtering by status (Pending, Confirmed, Cancelled, Completed)
- Combined filters
- Validation tests
- Complete workflow test (create account → create registration → verify → cleanup)

## 🎯 Usage Tips

### Variables

Each `.http` file uses variables for easy testing:

```http
@baseUrl = http://localhost:5000
@accountId = 
@newAccountId = 
```

**How to use variables:**

1. Execute a test that returns data (e.g., GET all accounts)
2. Copy an ID from the response
3. Paste it after the `=` sign in the variable declaration at the top
4. Subsequent tests will use that ID

**Example:**
```http
@accountId = 3c4f8e9b-2d1a-4b6c-9e8f-7a6b5c4d3e2f
```

### Named Requests

Some requests have names (e.g., `# @name createAccount`) which allows you to reference their responses in subsequent requests. This is useful for chaining tests together.

### Test Execution Order

For best results, follow this order:

**Accounts:**
1. GET all accounts (step 1) → copy an account ID
2. Update `@accountId` variable with the ID
3. Run GET account by ID (step 5)
4. Run CREATE account (step 6 or 7) → copy new ID
5. Update `@newAccountId` with new ID
6. Run UPDATE account (step 8)
7. Run DELETE account (step 11)

**Registrations:**
1. GET accounts first to get an account ID
2. Update `@accountId` variable
3. Run GET registrations tests (steps 1-8)
4. Run CREATE registration tests (steps 9-11)
5. Run complete workflow (steps 15-19)

### Running Multiple Tests

You can execute multiple tests in sequence:
1. Hold Ctrl (or Cmd on Mac)
2. Click "Send Request" on multiple tests
3. Each will open in a new response pane

## 🧪 Test Scenarios Covered

### Happy Path
- ✅ Create, Read, Update, Delete operations
- ✅ Pagination with different page sizes
- ✅ Search and filter functionality
- ✅ Related entity queries (account → registrations)

### Error Handling
- ✅ Validation failures (400 Bad Request)
- ✅ Not found errors (404 Not Found)
- ✅ Invalid GUIDs
- ✅ Missing required fields

### Edge Cases
- ✅ Optional fields (e.g., registration amount)
- ✅ Different enum values (statuses)
- ✅ Empty search results
- ✅ Non-existent entities

## 🔄 Complete Workflow Example

The `RegistrationsController.http` file includes a complete workflow test (steps 15-19):

1. **Create Account** - Creates a new test account
2. **Create Registration** - Adds a registration for that account
3. **Verify Account** - Gets the account with its registrations
4. **Get Registrations** - Lists all registrations for the account
5. **Cleanup** - Deletes the account (and its registrations)

This workflow simulates a real-world user journey through the API.

## 📝 Customization

### Change Base URL

If your API runs on a different port or protocol:

```http
@baseUrl = https://localhost:5001
```

### Add HTTPS Support

To test against HTTPS:

```http
@baseUrl = https://localhost:5001
```

### Add Headers

You can add custom headers to any request:

```http
GET {{baseUrl}}/api/accounts
Accept: application/json
Custom-Header: custom-value
```

## 🎨 REST Client Features

The REST Client extension supports many powerful features:

- **Environment Variables** - Switch between dev, staging, prod
- **Request History** - See previous requests and responses
- **Response Formatting** - JSON, XML, HTML rendering
- **Save Response** - Save responses to files
- **Code Generation** - Generate code in various languages
- **GraphQL Support** - Test GraphQL APIs
- **HTTP/2 Support** - Use HTTP/2 protocol

For more information, see the [REST Client documentation](https://github.com/Huachao/vscode-restclient).

## 🆚 REST Client vs Postman

| Feature | REST Client | Postman |
|---------|-------------|---------|
| Location | Inside VS Code | Separate app |
| File Format | Plain text `.http` | JSON collections |
| Version Control | Easy (text files) | Harder (JSON export) |
| Collaboration | Simple (commit files) | Requires Postman account |
| Scripting | Limited | Advanced (JavaScript) |
| Ease of Use | Very simple | More features, more complex |

**Use REST Client when:**
- You want to keep tests in version control with your code
- You prefer a simple, text-based approach
- You're already working in VS Code
- You want quick, lightweight API testing

**Use Postman when:**
- You need advanced scripting and test automation
- You need to share collections with non-developers
- You need comprehensive API testing features
- You want a dedicated API testing tool

## 📚 Additional Resources

- [REST Client Extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- [REST Client GitHub](https://github.com/Huachao/vscode-restclient)
- [HTTP Request Format](https://www.rfc-editor.org/rfc/rfc7230)
- [API Documentation](../README.md)

## 💡 Tips for Effective Testing

1. **Test Incrementally** - Run one test at a time to understand the API behavior
2. **Check Status Codes** - Verify the HTTP status code matches expectations
3. **Inspect Responses** - Review the response body structure and data
4. **Use Variables** - Set up variables to chain related tests together
5. **Test Error Cases** - Don't just test happy paths; test validation and errors
6. **Reset Data** - Restart the API to reset to seed data when needed
7. **Document Expected Results** - Add comments to describe what should happen
8. **Save Frequently** - The `.http` files are just text; save and commit them

## 🐛 Troubleshooting

### "Connection Refused" Error
- Make sure the API is running (`dotnet run`)
- Check the API is on the correct port (default: 5000)
- Verify `@baseUrl` matches your API URL

### "404 Not Found" on Valid Endpoint
- Verify the route is correct
- Check for typos in the URL
- Make sure the API started successfully

### Invalid GUID Format
- GUIDs must be in format: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
- Copy GUIDs directly from API responses
- Don't use made-up GUIDs for GET operations

### Variables Not Working
- Make sure you've set the variable value (after the `=`)
- Variable names are case-sensitive
- Variables must be defined before use in the file

## ✅ Testing Checklist

Before considering testing complete:

- [ ] All GET endpoints return 200 OK
- [ ] POST endpoints create resources and return 201 Created
- [ ] PUT endpoints update resources and return 200 OK
- [ ] DELETE endpoints remove resources and return 204 No Content
- [ ] Invalid data returns 400 Bad Request
- [ ] Non-existent resources return 404 Not Found
- [ ] Pagination works correctly
- [ ] Search/filter functionality works
- [ ] Related entity queries work (account → registrations)
- [ ] Validation errors are descriptive

Happy Testing! 🎉
