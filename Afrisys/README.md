# Afrisys.JwtAuthKit

A lightweight JWT authentication and API authorization library for ASP.NET Core.

Designed to simplify secure microservice communication using **JWT Audience-based isolation**, without complex policy or IdentityServer configuration.

[![NuGet Version](https://img.shields.io/nuget/v/Afrisys.JwtAuthKit.svg)](https://www.nuget.org/packages/Afrisys.JwtAuthKit)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Afrisys.JwtAuthKit.svg)](https://www.nuget.org/packages/Afrisys.JwtAuthKit)

---

## 📌 Why This Exists

Modern microservice systems often struggle with:

- Overly complex authentication configuration per service
- Weak service-to-service isolation
- Repeated JWT setup across APIs
- Difficult-to-maintain authorization rules

**Afrisys.JwtAuthKit** solves this by enforcing a simple principle:

> **JWT Audience defines API access boundaries**

Each API only accepts tokens explicitly intended for it.

---

## ✨ Features

- Minimal setup (single-line configuration)
- Automatic JWT Audience validation
- Works with any JWT provider (Duende, Auth0, Keycloak, Azure AD, etc.)
- Lightweight with zero unnecessary dependencies
- Compatible with Controllers and Minimal APIs
- Clean separation of authentication and authorization logic

---

## ⚡ Quick Start

### 1. Install Package

```bash
dotnet add package Afrisys.JwtAuthentication.AspNetCore
```

---

### 2. Configure `appsettings.json`

```json
{
  "Auth": {
    "Authority": "http://your-identity-server.com",
    "Audience": "Your scope here"
  }
}
```

---

### 3. Register in `Program.cs`

```csharp
using Afrisys.JwtAuthKit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthKit(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

### 4. Secure Your API

```csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlumbingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Secure data accessed successfully.");
    }
}
```

---

## 🔐 How It Works

1. Identity Provider issues a JWT token with an `aud` (audience) claim  
2. Each microservice defines its expected audience  
3. The library validates:
   - Token signature
   - Issuer (Authority)
   - Audience match  
4. If audience mismatch → request is rejected (401 Unauthorized)

---

## 🧭 Audience Mapping Example

| API Service          | Expected Audience       |
|---------------------|------------------------|
| Plumbing API        | `plumbing-api`         |
| Ecommerce API       | `ecommerce-api`        |
| Notification Service| `notification-service` |

---

## 🧪 Example Token Request (Client Credentials)

```bash
curl --location 'https://your-identity-server.com/connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=plumbing-api' \
--data-urlencode 'client_secret=your-secret' \
--data-urlencode 'grant_type=client_credentials'
```

### Response

```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer",
  "expires_in": 3600
}
```

---

## 🏗️ Architecture Overview

```
Client / Service
      ↓
Identity Provider (JWT Issuance)
      ↓
Afrisys.JwtAuthKit (Validation Layer)
      ↓
Protected API Resource
```

---

## ⚙️ Requirements

- .NET 8 or later  
- ASP.NET Core Web API  
- Any JWT-compatible Identity Provider  


## 📄 License

MIT License

---

## ❤️ Built For

Developers building secure, scalable, and clean microservice architectures.