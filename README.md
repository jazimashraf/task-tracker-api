# Task Tracker API

This is a small .NET Web API built to manage tasks.  
The goal was to keep the design clean, testable, and simple.

## Tech

- .NET Web API
- Entity Framework Core (SQLite)
- FluentValidation
- xUnit

## Structure

The solution is split into:

- Domain – core models
- Application – business logic, DTOs, interfaces
- Infrastructure – EF Core implementation
- API – controllers
- Tests – unit tests

Instead of using DbContext directly in the service layer, I introduced an `ITaskRepository`.  
This keeps the Application layer independent and easier to test.

## What it does

- Create, update, delete tasks
- Get all tasks or by id
- Basic validation and business rules

## Rules

- Title is required
- Title max length is 100
- Task cannot be marked as Done if title is empty

## Run it

```bash
dotnet restore
dotnet build
dotnet run --project TaskTracker.Api
```
## Swagger:
```bash
http://localhost:<port>/swagger
```

## Test

```bash
dotnet test
