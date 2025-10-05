# YamSoft Practical Task

A full-stack e-commerce application with .NET 8 Web API backend and React TypeScript frontend.

## Application Features

### Backend API Features
Built with .NET 8, the API handles authentication (JWT + BCrypt), product management (full CRUD with images, filtering, and inventory), and a persistent shopping cart system.
It includes foundation for role-based access, automatic DB seeding, notifications, and unit tests using mocked dependencies. Data access is managed with Entity Framework + MySQL and AutoMapper for clean DTOs.

### Frontend Features  
The React + TypeScript frontend uses Material-UI for a responsive design. It supports secure login, product browsing with search and filters, real-time cart updates, and smooth API integration via Axios with proper error handling and loading states.

### Infrastructure & Architecture
The app runs in Docker containers with MySQL, Nginx (for image hosting and reverse proxy), and phpMyAdmin. It supports automated migrations, environment configs, and a production-ready Docker Compose setup.

## Prerequisites

### Required Software
- **Docker Desktop** - Required to run the containerized database and services
- **.NET 8 SDK** - For running the API backend
- **Node.js 16+** - For running the frontend (optional)
- **Entity Framework Core Tools** - For database migrations

### Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

## Quick Setup

### 1. Environment Configuration

You'll notice there are two .env.example files: one for the frontend and another for docker-compose. The example files contain variables that are already configured to work with the application settings. You can modify them if needed, but it's not necessary unless you encounter errors.

#### Root Directory (.env)

Note: These variables are specifically for docker-compose configuration.

Copy `.env.example` to `.env` and configure:
```bash
# Database Configuration
MYSQL_ROOT_PASSWORD=YamSoft2024!
MYSQL_DATABASE=yamsoft_db
MYSQL_USER=yamsoft_user
MYSQL_PASSWORD=YamSoft123!

# Ports
MYSQL_PORT=3306
NGINX_PORT=8080
PHPMYADMIN_PORT=8081

# Image Server Configuration
IMAGES_BASE_URL=http://localhost:8080/images/
IMAGES_UPLOAD_PATH=../images/
```

#### Frontend (.env)

Note: This variable tells the frontend where to find the API. If you've modified `API/Properties/launchSettings.json`, this URL may be different.

Copy `frontend/.env.example` to `frontend/.env`:
```bash
REACT_APP_API_URL=https://localhost:7272/api
```

### 2. Start Docker Services

Start the containerized database and nginx server:
```bash
docker-compose up -d
```

This will start:
- **MySQL Database** (port 3306) - Containerized for easy setup and consistent environment
- **Nginx Server** (port 8080) - Serves static images with CORS headers
- **phpMyAdmin** (port 8081) - Database management interface

Note: You can and should access phpMyAdmin to check how the database is doing! It has feelings too, you know..

### 3. Run Database Migrations

Navigate to the API directory and run migrations:
```bash
cd API
dotnet ef database update
```

### 4. Start the Backend API

Run the .NET API:
```bash
dotnet run
# or for development with hot reload:
dotnet watch run
```

The API will be available at:
- HTTP: http://localhost:5121
- HTTPS: https://localhost:7272
- Swagger UI: https://localhost:7272/swagger

Note: The API is using HTTPSRedirection, so most likely you will always be redirected to the HTTPS url.

## Frontend (Optional)

### Install Dependencies
```bash
cd frontend
npm install
```

### Run Development Server
```bash
npm start
```

The frontend will be available at http://localhost:3000

### Build for Production
```bash
npm run build
```

## Configuration Files

### appsettings.json
Contains:
- Database connection strings
- JWT token configuration  
- Image server settings
- Logging configuration

### appsettings.Development.json
Development-specific overrides for local development.

## Why Multiple Environment/Configuration Files?

There was consideration to containerize everything (the API and frontend) in docker-compose with a single environment configuration, but this would require more time and resources than currently available. Applications like the frontend don't have access to the root directory, meaning they cannot use the same variables as docker-compose. 

## Why Containerized?

### MySQL Database
- **Consistent Environment**: Same database version across all development machines
- **Easy Setup**: No need to install MySQL locally
- **Isolation**: Doesn't conflict with other projects
- **Data Persistence**: Uses Docker volumes to preserve data

### Nginx
- **Static File Serving**: Efficiently serves product images
- **CORS Headers**: Configured to handle cross-origin requests from frontend

## Why Nginx?

Based on research and best practices, it's always better to store images in cloud storage or a dedicated file server, saving only the URL or path in the database. Storing images directly in the database would slow down database operations and create challenges for horizontal scaling. Nginx serves as a proxy for serving static images efficiently. In an ideal scenario, Nginx would also handle load balancing and serve as a proxy for the backend API, but the backend would need to be containerized first before implementing that architecture.

## Database Setup

The database is automatically seeded with mock data when the API starts if the database is empty. Ensure the database is running in Docker (or elsewhere) with the correct credentials and has been updated using the Entity Framework CLI tool.

## Unit Tests

The project includes unit tests for the API that can be run using the `dotnet test` command. These are simple xUnit tests demonstrating how to mock database contexts and configuration files for testing purposes.

You will notice the tests passing (or failing) in the github actions tab. This project does have a github workflow to run the tests when pulling into main.

## Available Commands

### .NET API
```bash
# Build the project
dotnet build

# Run with hot reload
dotnet watch run

# Run tests
dotnet test

# Entity Framework migrations
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Frontend
```bash
# Start development server
npm start

# Build for production
npm run build
```

### Docker
```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down

# View logs
docker-compose logs

# Rebuild services
docker-compose up --build
```

## Project Structure

- `API/` - .NET 8 Web API backend
- `frontend/` - React TypeScript frontend
- `docker-compose.yml` - Container orchestration
- `nginx/` - Nginx configuration
- `images/` - Static image files

## Troubleshooting

1. **Database connection issues**: Ensure Docker services are running.
2. **Port conflicts**: Check if ports 3306, 8080, 8081, 5000, 7272 are available.
3. **EF migrations**: Run `dotnet ef database update` in the API directory.
4. **CORS issues**: Verify nginx is running for image serving (If Nginx or Product troubles). Or check if the frontend is running on port 3000. If not you will have to go into API/Program.cs and change the CORS policy into the correct port.