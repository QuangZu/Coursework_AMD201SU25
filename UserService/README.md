# Express.js Authentication Service

A microservice for handling user authentication in the URL shortener application.

## Features

- ✅ User registration with email and password
- ✅ User login with JWT token generation
- ✅ Password hashing with bcrypt
- ✅ JWT token verification
- ✅ User profile retrieval
- ✅ Input validation
- ✅ Rate limiting
- ✅ CORS support
- ✅ Security headers with Helmet

## API Endpoints

### Register User
```
POST /api/auth/register
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "password123"
}
```

### Login User
```
POST /api/auth/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "password123"
}
```

### Verify Token
```
GET /api/auth/verify
Authorization: Bearer <token>
```

### Get User Profile
```
GET /api/auth/profile
Authorization: Bearer <token>
```

## Setup

### 1. Install Dependencies
```bash
cd UserService
npm install
```

### 2. Environment Variables
Create a `.env` file:
```env
PORT=5001
NODE_ENV=development
DB_HOST=localhost
DB_PORT=5432
DB_NAME=postgres
DB_USER=postgres
DB_PASSWORD=postgres
JWT_SECRET=your-super-secret-jwt-key-change-in-production
```

### 3. Run with Docker
```bash
# Build and run with docker-compose
docker-compose up auth

# Or build and run individually
docker build -t auth-service .
docker run -p 5001:5001 auth-service
```

### 4. Run Locally
```bash
npm start
# or for development
npm run dev
```

## Integration with Ocelot Gateway

The authentication service is integrated with the Ocelot Gateway at:
- **Gateway URL**: `http://localhost:8000/auth/*`
- **Service URL**: `http://localhost:5001/api/auth/*`

## Database Schema

The service creates a `users` table in PostgreSQL:

```sql
CREATE TABLE users (
  id SERIAL PRIMARY KEY,
  username VARCHAR(50) UNIQUE NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

## Security Features

- **Password Hashing**: Uses bcrypt with salt rounds
- **JWT Tokens**: Secure token-based authentication
- **Rate Limiting**: Prevents brute force attacks
- **Input Validation**: Validates all user inputs
- **CORS**: Configured for frontend integration
- **Helmet**: Security headers

## Testing

### Test Registration
```bash
curl -X POST http://localhost:8000/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "password123"
  }'
```

### Test Login
```bash
curl -X POST http://localhost:8000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123"
  }'
```

### Test Token Verification
```bash
curl -X GET http://localhost:8000/auth/verify \
  -H "Authorization: Bearer <your-token>"
``` 