# Account Registration API Documentation

This document provides quick, practical examples for using the API.

## Base URL

```text
http://localhost:5000
```

## Run the API

```bash
dotnet restore
dotnet run --urls "http://localhost:5000"
```

## Accounts

### List accounts (with optional search + pagination)

```bash
curl -X GET "http://localhost:5000/api/accounts?page=1&pageSize=5&search=jane"
```

### Create account

```bash
curl -X POST "http://localhost:5000/api/accounts" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Ava",
    "lastName": "Morgan",
    "email": "ava.morgan@example.com",
    "phone": "+1-555-0100",
    "address": "100 Main Street",
    "status": "Active"
  }'
```

### Get account by ID

```bash
curl -X GET "http://localhost:5000/api/accounts/{accountId}"
```

### Update account

```bash
curl -X PUT "http://localhost:5000/api/accounts/{accountId}" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Ava",
    "lastName": "Morgan",
    "email": "ava.updated@example.com",
    "phone": "+1-555-0101",
    "address": "120 Main Street",
    "status": "Inactive"
  }'
```

### Delete account

```bash
curl -X DELETE "http://localhost:5000/api/accounts/{accountId}"
```

### Get registrations for account

```bash
curl -X GET "http://localhost:5000/api/accounts/{accountId}/registrations"
```

## Registrations

### List registrations (optional account + status filters)

```bash
curl -X GET "http://localhost:5000/api/registrations?page=1&pageSize=10&status=Confirmed"
curl -X GET "http://localhost:5000/api/registrations?accountId={accountId}"
```

### Create registration

```bash
curl -X POST "http://localhost:5000/api/registrations" \
  -H "Content-Type: application/json" \
  -d '{
    "accountId": "{accountId}",
    "status": "Pending",
    "eventOrCourseName": "C# Fundamentals Bootcamp",
    "amount": 199.99,
    "details": "Weekday evening cohort"
  }'
```

## Common response examples

### Paginated response shape

```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "data": []
}
```

### Validation error shape

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": [
      "A valid email address is required."
    ]
  }
}
```

## Valid enum values

- `AccountStatus`: `Active`, `Inactive`, `Suspended`
- `RegistrationStatus`: `Pending`, `Confirmed`, `Cancelled`, `Completed`
