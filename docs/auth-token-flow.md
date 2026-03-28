# Authentication Token Flow

## Overview

- **Access Token**: Stored in memory (JavaScript variable), sent as `Authorization: Bearer` header
- **Refresh Token**: Stored as `HttpOnly`, `Secure`, `SameSite=Strict` cookie, managed by browser automatically

---

## Login Flow

```mermaid
sequenceDiagram
    participant U as User
    participant FE as Angular Frontend
    participant BE as .NET Backend
    participant DB as Database

    U->>FE: Enter credentials
    FE->>BE: POST /api/Auth/login<br/>{username, password}
    BE->>DB: Find user by username
    DB-->>BE: User record
    BE->>BE: Verify password hash
    BE->>BE: Generate access token (JWT, 1h)
    BE->>BE: Generate refresh token (random)
    BE->>DB: Save refresh token (7d expiry)
    BE-->>FE: Response body: {accessToken, expiresAt, user}<br/>Set-Cookie: refreshToken=xxx; HttpOnly; Secure; SameSite=Strict
    FE->>FE: Store accessToken in memory
    FE->>FE: Store user in localStorage
    FE->>U: Redirect to dashboard
```

## Authenticated Request Flow

```mermaid
sequenceDiagram
    participant FE as Angular Frontend
    participant INT as Auth Interceptor
    participant BE as .NET Backend

    FE->>INT: HTTP request
    INT->>INT: Read accessToken from memory
    INT->>BE: Request + Authorization: Bearer {accessToken}
    BE->>BE: Validate JWT
    BE-->>INT: 200 OK (response data)
    INT-->>FE: Response data
```

## Token Refresh Flow (on 401)

```mermaid
sequenceDiagram
    participant FE as Angular Frontend
    participant INT as Auth Interceptor
    participant BE as .NET Backend
    participant DB as Database

    FE->>INT: HTTP request
    INT->>BE: Request + Authorization: Bearer {expired accessToken}
    BE-->>INT: 401 Unauthorized

    Note over INT: Interceptor catches 401

    INT->>BE: POST /api/Auth/refresh-token<br/>Cookie: refreshToken=xxx (sent by browser)
    BE->>DB: Find refresh token (not revoked, not expired)
    DB-->>BE: Stored token record
    BE->>DB: Revoke old refresh token
    BE->>BE: Generate new access token
    BE->>BE: Generate new refresh token
    BE->>DB: Save new refresh token
    BE-->>INT: Response body: {accessToken, expiresAt, user}<br/>Set-Cookie: refreshToken=newXxx; HttpOnly; Secure; SameSite=Strict
    INT->>INT: Store new accessToken in memory

    Note over INT: Retry original request

    INT->>BE: Original request + Authorization: Bearer {new accessToken}
    BE-->>INT: 200 OK (response data)
    INT-->>FE: Response data
```

## Concurrent Requests During Refresh

```mermaid
sequenceDiagram
    participant R1 as Request 1
    participant R2 as Request 2
    participant INT as Auth Interceptor
    participant BE as .NET Backend

    R1->>INT: Request (gets 401)
    R2->>INT: Request (gets 401)

    Note over INT: isRefreshing = true<br/>Only Request 1 triggers refresh

    INT->>BE: POST /api/Auth/refresh-token
    Note over R2,INT: Request 2 waits on refreshTokenSubject

    BE-->>INT: New tokens
    INT->>INT: isRefreshing = false
    INT->>INT: refreshTokenSubject.next(newAccessToken)

    Note over INT: Both requests retry with new token

    INT->>BE: Retry Request 1 with new token
    INT->>BE: Retry Request 2 with new token
    BE-->>R1: Response
    BE-->>R2: Response
```

## Logout Flow

```mermaid
sequenceDiagram
    participant U as User
    participant FE as Angular Frontend
    participant BE as .NET Backend

    U->>FE: Click logout
    FE->>FE: Clear accessToken from memory
    FE->>FE: Clear user from localStorage
    FE->>BE: POST /api/Auth/logout (withCredentials)
    BE->>BE: Delete refreshToken cookie
    BE-->>FE: 200 OK
    FE->>U: Redirect to login
```

## Security Architecture

```mermaid
flowchart TD
    subgraph Browser
        MEM["Memory (JS variable)<br/>accessToken"]
        COOKIE["HttpOnly Cookie<br/>refreshToken"]
        LS["localStorage<br/>user profile only"]
    end

    subgraph Protection
        XSS["XSS Attack"]
        CSRF["CSRF Attack"]
    end

    XSS -. "cannot read" .-> COOKIE
    XSS -. "can read but<br/>short-lived (1h)" .-> MEM
    CSRF -. "blocked by<br/>SameSite=Strict" .-> COOKIE

    style COOKIE fill:#2d6a4f,color:#fff
    style MEM fill:#1b4332,color:#fff
    style LS fill:#40916c,color:#fff
    style XSS fill:#d00000,color:#fff
    style CSRF fill:#d00000,color:#fff
```

## Token Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Login: User authenticates

    Login --> Active: Access token in memory<br/>Refresh token in cookie

    Active --> Active: API requests with Bearer token

    Active --> Refreshing: Access token expired (401)

    Refreshing --> Active: Refresh successful<br/>New tokens issued<br/>Old refresh token revoked

    Refreshing --> LoggedOut: Refresh failed<br/>(expired/revoked)

    Active --> LoggedOut: User clicks logout

    LoggedOut --> [*]: Cookie cleared<br/>Memory cleared
```
