# URL History Integration

This document describes the integration between URLShorteningService and UserService for tracking URL shortening history.

## Overview

When a user shortens a URL through the URLShorteningService, the system now automatically adds the URL to the user's history in the UserService. This integration works through the OcelotGateway.

## Changes Made

### URLShorteningService

1. **Updated URLShorteningController.cs**:
   - Added `UserId` property to `UrlRequest` class
   - Added `AddToHistory` method that sends HTTP requests to UserService
   - Modified `ShortenUrl` method to call `AddToHistory` when `UserId` is provided
   - Added HttpClient dependency injection

2. **Updated Program.cs**:
   - Added `HttpClient` service registration

### UserService

1. **Updated userController.js**:
   - Modified `getProfile` method to include user's URL history
   - Added `addToHistory` method to handle history storage
   - Updated module exports to include `addToHistory`

2. **Updated userRoute.js**:
   - Added POST `/history` route for receiving history data from URLShorteningService

3. **Updated database.js**:
   - Added `url_history` table creation with foreign key to users table

### OcelotGateway

- No changes needed - existing `/user/{everything}` route handles the new `/user/history` endpoint

## API Usage

### Shortening URL with User History

```http
POST /shorten
Content-Type: application/json

{
  "longUrl": "https://www.example.com",
  "userId": 1
}
```

### Getting User Profile with History

```http
GET /user/profile
Authorization: Bearer <jwt_token>
```

Response includes:
```json
{
  "user": {
    "id": 1,
    "username": "user1",
    "email": "user@example.com",
    "created_at": "2025-01-27T..."
  },
  "history": [
    {
      "long_url": "https://www.example.com",
      "short_url": "abc123",
      "created_at": "2025-01-27T..."
    }
  ]
}
```

## Database Schema

### url_history table
```sql
CREATE TABLE url_history (
  id SERIAL PRIMARY KEY,
  user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  long_url TEXT NOT NULL,
  short_url VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

## Flow

1. User sends URL shortening request with `userId`
2. URLShorteningService creates short URL
3. URLShorteningService calls UserService via OcelotGateway to add to history
4. UserService stores the URL in `url_history` table
5. User can view history via `/user/profile` endpoint

## Error Handling

- If history storage fails, the URL shortening operation still succeeds
- History storage errors are logged but don't affect the main functionality
- Missing `userId` simply skips history storage