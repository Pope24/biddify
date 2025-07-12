# Global Exception Handling

This library provides a standardized way to handle exceptions throughout the application.

## Usage

### 1. Setting up the middleware

The global exception handling middleware is already set up in the `Program.cs` file. 
It will automatically catch and process all exceptions that occur during HTTP requests.

### 2. Custom Exceptions

The following custom exceptions are available:

- `ApiException` - Base exception for all API errors (status code 500)
- `BadRequestException` - For client errors like validation errors (status code 400)
- `UnauthorizedException` - For authentication failures (status code 401)
- `ForbiddenException` - For authorization failures (status code 403)
- `NotFoundException` - For resources that don't exist (status code 404)

### 3. Extension Methods

You can use these helper extension methods to throw exceptions in a cleaner way:

```csharp
// Check if an object is null and throw NotFoundException if it is
user.ThrowIfNull("User not found");

// Check if a collection is null or empty and throw NotFoundException if it is
products.ThrowIfNullOrEmpty("No products found");

// Check a condition and throw BadRequestException if it's false
isValid.ThrowIfFalse("Invalid data provided");

// Check a condition and throw BadRequestException if it's true
isDeleted.ThrowIfTrue("Product is already deleted");
```

### 4. Example Usage

```csharp
public async Task<UserEntity> GetUserByIdAsync(string id)
{
    // Throws NotFoundException if id is null
    id.ThrowIfNull("User ID cannot be null");
    
    var user = await userRepository.GetUserByIdAsync(id);
    
    // Throws NotFoundException if user is null
    user.ThrowIfNull($"User with ID {id} not found");
    
    return user;
}
```

### 5. Response Format

When an exception is thrown and captured by the middleware, the response will have this format:

```json
{
  "statusCode": 404,
  "message": "User with ID abc123 not found",
  "traceId": "0HM6Q1KOPHOFT:00000001",
  "timestamp": "2024-06-22T12:34:56.789Z",
  "errors": {}
}
```

The `errors` dictionary can be used for validation errors with field names as keys and error messages as values. 