# Boilerplate.Beta.Core

Boilerplate.Beta.Core is a modular and scalable backend boilerplate built with .NET. It supports REST APIs, Kafka, WebSockets (SignalR), and integrates out of the box with [FusionAuth](https://fusionauth.io) for identity management.

Designed for fast onboarding and reliable local/Docker development, this boilerplate lays the foundation for clean architecture and service-oriented systems.

## Features
- **Docker-Ready**: Preconfigured for Docker-based local development.

- **FusionAuth Integration**: Seamlessly integrated with FusionAuth for user authentication and authorization.

- **REST API**: Scaffold APIs with ASP.NET Core controllers and DTOs.

- **Kafka**: Built-in Kafka producer/consumer setup for event-driven systems.

- **WebSockets (SignalR)**: Real-time communication using SignalR.

- **Custom Logger & Error Handler**: Unified logging strategy and global exception filters.

## Installation

##### 1. Clone the repositories:


```sh
git clone https://github.com/yourusername/Boilerplate.Beta.Core.git
git clone https://github.com/yourusername/Boilerplate.Beta.Frontend.git
git clone https://github.com/yourusername/Boilerplate.Beta.Infrastructure.git
```

- *[Boilerplate.Beta.Infrastructure](https://github.com/GkChris/Boilerplate.Beta.Infrastructure)*

- *[Boilerplate.Beta.Frontend](https://github.com/GkChris/Boilerplate.Beta.Frontend)*

##### 2. Navigate to the infrastructure scripts:

```sh
cd Boilerplate.Beta.Infrastructure/scripts
```

##### 3. Start your environment:

- Full stack via Docker (Core runs in container):

```sh
./setup.sh full
```

- Infrastructure only (Run Core manually via Visual Studio):

```sh
./setup.sh base
```

## Usage

Build REST endpoints using standard ASP.NET controllers.

Integrate Kafka producers/consumers via the built-in config.

Use SignalR hubs for real-time push updates.

Identity and auth flows go through FusionAuth (configure your tenant if needed).

## License

Boilerplate.Beta.Core is licensed under the MIT License. See [LICENSE](https://github.com/GkChris/Boilerplate.Beta.Core?tab=MIT-1-ov-file) for more details.