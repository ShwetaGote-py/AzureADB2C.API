# Azure AD B2C Authentication API

This project implements Azure AD B2C authentication using the Resource Owner Password Credentials (ROPC) flow in a .NET 8 Web API. It provides secure authentication endpoints with JWT token support.

## Features

- ROPC (Resource Owner Password Credentials) flow implementation
- JWT token authentication
- Token validation endpoint
- Swagger UI integration
- Clean architecture implementation
- Secure password handling

## Prerequisites

1. .NET 8.0 SDK
2. Azure AD B2C tenant
3. Visual Studio 2022 or VS Code
4. Azure subscription

## Project Structure

```
AzureADB2C.API/
├── Controllers/
│   └── AuthController.cs        # Authentication endpoints
├── Models/
│   ├── AuthenticationResult.cs  # Token response model
│   └── LoginRequest.cs         # Login request model
├── Services/
│   ├── IAuthenticationService.cs           # Auth service interface
│   └── AzureAdB2CAuthenticationService.cs  # B2C implementation
└── Program.cs                  # Application configuration
```

## Configuration

Update `appsettings.json` with your Azure AD B2C settings:

```json
{
  "AzureAdB2C": {
    "Instance": "https://your-tenant.b2clogin.com",
    "ClientId": "your-client-id",
    "Domain": "your-tenant.onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1_signupsignin",
    "TenantId": "your-tenant-id",
    "ClientSecret": "your-client-secret",
    "Scopes": [
      "https://your-tenant.onmicrosoft.com/api/read",
      "https://your-tenant.onmicrosoft.com/api/write"
    ]
  }
}
```

## Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd AzureADB2C.API
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

## API Endpoints

### 1. Login
- **URL**: `/api/auth/login`
- **Method**: `POST`
- **Headers**: 
  - Content-Type: application/json
- **Body**:
  ```json
  {
    "username": "user@example.com",
    "password": "password123"
  }
  ```
- **Response**:
  ```json
  {
    "accessToken": "eyJ0eXAiOiJKV...",
    "tokenType": "Bearer",
    "expiresIn": 3600,
    "refreshToken": "eyJ0eXAiOiJKV...",
    "idToken": "eyJ0eXAiOiJKV..."
  }
  ```

### 2. Validate Token
- **URL**: `/api/auth/validate`
- **Method**: `GET`
- **Headers**: 
  - Authorization: Bearer {token}
- **Response**:
  ```json
  true/false
  ```

## Security Features

1. JWT token validation
2. Secure password handling
3. HTTPS enforcement
4. Input validation
5. Error logging
6. Rate limiting (configurable)

## Azure AD B2C Setup

1. Create an Azure AD B2C tenant
2. Register your application
3. Configure user flows
4. Set up ROPC flow
5. Configure API scopes

## Best Practices

1. Store sensitive configuration in Azure Key Vault
2. Use HTTPS in production
3. Implement proper logging
4. Monitor authentication attempts
5. Regularly rotate secrets
6. Use secure password policies

## Testing

1. Use Swagger UI for API testing
   - Navigate to `https://localhost:5001/swagger`
   - Try the login and validate endpoints

2. Use Postman:
   - Import the provided Postman collection
   - Set up environment variables
   - Run the authentication tests

## Error Handling

The API implements proper error handling for:
- Invalid credentials
- Expired tokens
- Network issues
- Server errors
- Validation errors

## Deployment

1. Azure App Service deployment:
   ```bash
   dotnet publish -c Release
   ```

2. Configure environment variables
3. Set up SSL certificates
4. Enable logging and monitoring
5. Configure scaling rules

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please:
1. Check the documentation
2. Create an issue
3. Contact the development team

## Acknowledgments

- Microsoft Identity Platform
- Azure AD B2C Team
- .NET Community
