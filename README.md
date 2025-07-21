# Boilerplate.Beta.Core

## 1. Project Overview

Boilerplate.Beta.Core is a modular and scalable backend boilerplate
built with .NET. It supports `REST APIs`, `Kafka`, `WebSockets (SignalR)`, and
integrates out of the box with [FusionAuth](https://fusionauth.io) for identity management.  
  
Designed for fast onboarding and reliable local/Docker development, this
boilerplate lays the foundation for clean architecture and
service-oriented systems.

## 2. Architecture

Boilerplate.Beta.Core follows a clean, modular architecture:  
  
- **Application**: Contains core logic, DTOs, service abstractions,
controllers, and middleware.  
- **Data**: Handles Entity Framework DbContext and seeders.
Migrations are expected to be placed under `Data/Migrations`, which is
excluded from source control via `.gitignore`. Developers must perform
an initial migration.  
- **Infrastructure**: Contains integrations with external systems
such as Kafka, SignalR, FusionAuth, Swagger, and CORS, as well as
DI/service registration.  
  
The architecture is designed for testability, extensibility, and clear
separation of concerns.

## 3. Environments

**Development.Local**: Run the backend independently using Visual
Studio or `dotnet run`. Uses `appsettings.Development.Local.json`.  
  
**Development.Docker**: Core runs inside a container and uses
`appsettings.Development.Docker.json`. Environment values are
overwritten by Docker Compose from the infrastructure repo.  
  
**Production**: Core runs inside a container and uses
`appsettings.json`. Environment values are
overwritten by Docker Compose from the infrastructure repo.

## 4. Installation & Usage

1. Clone the repositories:

> git clone https://github.com/gkchris/Boilerplate.Beta.Core.git  
> git clone
> https://github.com/gkchris/Boilerplate.Beta.Frontend.git  
> git clone
> https://github.com/gkchris/Boilerplate.Beta.Infrastructure.git

2. Navigate to the infrastructure scripts:

> cd Boilerplate.Beta.Infrastructure/scripts

3. Start your environment:

> • Full stack via Docker (Core runs in container): ./setup.sh full  
> • Infrastructure only (Run Core manually via Visual Studio):
> ./setup.sh base

## 5. Authentication & Identity

Authentication is delegated to the frontend, which communicates with
FusionAuth for login, registration, logout, and refresh token handling.
It sends requests with an HTTP-only cookie containing the access
token.  
  
The backend authorizes users based on this cookie and optionally
supports tokens in headers, controlled by flags in
`appsettings.json`.  
  
The relevant code resides in
`Infrastructure/Extensions/AuthExtension.cs`.

## 6. Messaging (Kafka)

Kafka is configured under `Infrastructure/Messaging/Kafka`. The
`KafkaPublisherService` in `Application/Services` handles message
publishing, while the `KafkaMessageHandler` in
`Application/Handlers` processes incoming events.

## 7. WebSockets (SignalR)

SignalR is set up in `Infrastructure/Messaging/SignalR`. Real-time events are
published using `SignalRPublisherService` in `Application/Services`,
and handled using `SignalRMessageHandler` in `Application/Handlers`.

## 8. Error Handling & Logging

Custom logger and a global error handler are integrated to provide
consistent logging and exception responses across the API.  
  
Custom logging and error handling middleware implementations are located
under `Application/Middlewares`.

## 9. Project Structure

**/Application** - Use-case logic, service abstractions, DTOs, controllers  
**/Data** - DB context, EF seeders  
**/Infrastructure** - Configurations for Kafka, SignalR, FusionAuth,
Swagger, CORS, service registration, etc.

## 10. Configuration

Configuration files include:  
- appsettings.json  
- appsettings.Development.Local.json  
- appsettings.Development.Docker.json  
Environment-specific settings and secrets should be injected through
environment variables when containerized.  
  
Database migrations must be created manually and stored in
`Data/Migrations`, which is ignored by git. If `AutoApplyMigrations`
is set to `true` in `appsettings.json`, migrations will be applied
automatically at runtime. Otherwise, run `dotnet ef database update`
manually.

## 11. Extending the Boilerplate

To add functionality:  
- Define new endpoints in the appropriate controller.  
- Publish events using `KafkaPublisherService` or
`SignalRPublisherService`.  
- Handle events using `KafkaMessageHandler` or
`SignalRMessageHandler`.  
- Register new services or repositories in
`Infrastructure/Extensions/ApplicationServiceExtensions.cs`.

## 12. Common Pitfalls

Coming soon

## 13. Testing

No tests are currently implemented under
`Boilerplate.Beta.Core.Tests`. Testing strategy is left to the
developer.

## 14. License

Boilerplate.Beta.Core is licensed under the MIT License. See [LICENSE](https://github.com/GkChris/Boilerplate.Beta.Core?tab=MIT-1-ov-file) for more details.