# Boilerplate.Beta.Core

Boilerplate.Beta.Core is a flexible .NET Core boilerplate designed to accelerate backend development. It is preconfigured to work seamlessly in both local and Docker environments and provides foundational components for building scalable, service-oriented applications.

## Features
- **Docker & Local Network Support**: Pre-configured for development in both local and Dockerized environments, making it easy to integrate with your infrastructure setup.

- **REST API**: Built-in implementation for creating and consuming REST APIs.

- **Kafka Integration**: Ready-to-use setup for integrating Kafka, enabling real-time messaging and event-driven architectures.

- **WebSockets with SignalR**: A built-in SignalR implementation to enable WebSocket-based communication for real-time updates.

- **Custom Logger & Error Handler**: Includes a custom logging mechanism and a centralized error handling solution to maintain consistency across services.

## Installation

To get started, you need to clone the following repositories.

##### 1. Clone the repositories:


```sh
git clone https://github.com/yourusername/Boilerplate.Beta.Core.git
git clone https://github.com/yourusername/Boilerplate.Beta.Frontend.git
git clone https://github.com/yourusername/Boilerplate.Beta.Infrastructure.git
```

- *[Boilerplate.Beta.Infrastructure](https://github.com/GkChris/Boilerplate.Beta.Infrastructure)*

- *[Boilerplate.Beta.Frontend](https://github.com/GkChris/Boilerplate.Beta.Frontend)*

##### 2. Navigate to the scripts directory within the Boilerplate.Beta.Infrastructure repository:

```sh
cd Boilerplate.Beta.Infrastructure/scripts
```

##### 3. Run one of the setup scripts:

- To run Boilerplate.Beta.Core on the Docker network, execute:

```sh
./setup.sh full
```

- To run only the other components and then run Boilerplate.Beta.Core via Visual Studio, execute:

```sh
./setup.sh base
```

## Usage

This boilerplate provides a ready-to-use framework with essential services. You can customize and extend it according to your project's needs.

**REST API**: Use standard controllers and routes to build your API endpoints.

**Kafka**: Easily set up Kafka producers and consumers for message-driven architectures.

**SignalR**: Leverage SignalR for WebSocket-based real-time communication.

## License

Boilerplate.Beta.Core is licensed under the MIT License. See [LICENSE](https://github.com/GkChris/Boilerplate.Beta.Core?tab=MIT-1-ov-file) for more details.