# Deveel Events

This project provides a simple and lightweight framework for publishing events to subscribers, using common channels and topics.

The ambition of this little framework is to implement a set of common patterns and practices that can be used to implement a simple and efficient event-driven architecture in a .NET application, using common approaches to create events and publish them.

It is not in the scope of this project to provide a full-featured event storage system, nor a complex pub/sub application: if you need such a system, you should consider using a message broker or a message queue system (such as RabbitMQ, Kafka, or Azure Service Bus) to implement a more complex and scalable event-driven architecture).

## Motivation

Often when developing applications, it is necessary to implement a mechanism to notify other parts of the system about changes or events that occur in the application: several times the implementor ends up rewriting boilerplate code to manage the events and notifications.

At the present time, there are several ways to implement such a mechanism, such as using a message broker, a message queue, or a pub/sub system, but every organization should implement its own way to manage events and notifications.

With this small effort, we aim to provide a simple and lightweight framework that can be used to implement a common way to publish events in a .NET application.

## CloudEvents Standard

The framework is designed to be compliant with the [CloudEvents](https://cloudevents.io/) standard, making use of the `CloudEvent` class to represent the reference model for the event.

This choice is made to ensure the maximum compatibility with other systems and services that are compliant with the standard, and to provide a simple and efficient way to serialize and deserialize events.

## Installation

The framework is available as a NuGet package, and can be installed using the `dotnet` cli.

For example, to install the package that provides the publisher to the Azure Service Bus, you can run the following command:

```bash
dotnet add package Deveel.Events.Publisher.AzureServiceBus
```

Alternatively, you can use the NuGet package manager in Visual Studio to search and install the package.

### Framework Packages

Packages provided by the framework are:

| Package | Description | NuGet Package | Pre-Release<br/>(GitHub Packages) |
|---------|-------------|---------------|-------------------------------|
| `Deveel.Events.Annotations` | A set of attributes used to describe the metadata of an event | [![NuGet](https://img.shields.io/nuget/v/Deveel.Events.Annotations.svg)](https://www.nuget.org/packages/Deveel.Events.Annotations) | [![GitHub](https://img.shields.io/badge/nuget-prerelease-yellow?logo=nuget)](https://github.com/deveel/deveel.events/pkgs/nuget/Deveel.Events.Annotations) |
| `Deveel.Events.Publisher` | The core framework for publishing events | [![NuGet](https://img.shields.io/nuget/v/Deveel.Events.Publisher.svg)](https://www.nuget.org/packages/Deveel.Events.Publisher) | [![GitHub](https://img.shields.io/badge/nuget-prerelease-yellow?logo=nuget)](https://github.com/deveel/deveel.events/pkgs/nuget/Deveel.Events.Publisher) |
| `Deveel.Events.Publisher.AzureServiceBus` | An implementation of the publisher using Azure Service Bus | [![NuGet](https://img.shields.io/nuget/v/Deveel.Events.Publisher.AzureServiceBus.svg)](https://www.nuget.org/packages/Deveel.Events.Publisher.AzureServiceBus) | [![GitHub](https://img.shields.io/badge/nuget-prerelease-yellow?logo=nuget)](https://github.com/deveel/deveel.events/pkgs/nuget/Deveel.Events.Publisher.AzureServiceBus) |

## Usage

The basic usage of the framework is to create an event, publish it to a channel, and subscribe to the channel to receive the event.

The following example shows how to create an event, publish it to a channel, and subscribe to the channel to receive the event.

To enable this capability, you must first register the publisher in the service collection of your application:

```csharp
using Deveel.Events;

var builder = WebApplication.CreateBuilder(args);

// ...

builder.Services.AddEventPublisher();
```

Then, you can create an event and publish it to a channel:

```csharp
using Deveel.Events;

public class MyService {
    private readonly IEventPublisher publisher;

    public MyService(IEventPublisher publisher) {
        this.publisher = publisher;
    }

    public async Task PublishEventAsync() {
        var @event = new CloudEvent("com.example.myevent", new Uri("http://example.com/events/123"), "Hello, World!") {
            ContentType = "text/plain",
            DataSchema = new Uri("http://example.com/schema"),
            Source = new Uri("http://example.com"),
            Data = "Hello, World!"
        };

        await publisher.PublishEventAsync(@event);
    }
}
```

Note that the above example will publish the event to all the channels that are registered in the publisher.

### Publishing from Event Data

If you have a class that represents the data of an event, you can use the `EventAttribute` to decorate such a class to describe the metadata of the event containing it.

For example, consider the following class:

```csharp
using Deveel.Events.Annotations;

[Event("com.example.myevent", "1.0")]
public class MyEventData {
    [Required]
    public string Message { get; set; }
}
```

You can then publish an event using the data class:

```csharp
using Deveel.Events;

public class MyService {
    private readonly IEventPublisher publisher;

    public MyService(IEventPublisher publisher) {
        this.publisher = publisher;
    }
    
    public async Task PublishEventAsync() {
        var data = new MyEventData {
            Message = "Hello, World!"
        };
        
        await publisher.PublishAsync(data);
    }
}
```

## Future Work

The framework is still in its early stages, and there are several areas that need to be improved and extended.

Some of the areas that we plan to work on in the future are:

- Supporting custom event serializers and deserializers
- Implementing more publishers for different messaging systems (eg. RabbitMQ, Kafka, etc.)
- Supporting the deserialization of events from channels, to make consistent the published events are consumed
- Allow the selection of the channel to publish an event among the registered ones (eg. with named channels)

You can monitor the list of [open issues](https://github.com/deveel/deveel.events/issues) to see what we are working on and what we plan to do in the future.

## Contributing

We welcome contributions to the project, and we encourage you to submit issues and pull requests to help us improve the framework.

If you want to contribute to the project, please read the [Contributing Guidelines](CONTRIBUTING.md) file to understand how to contribute to the project.

## License

The project is released under the [MIT License](LICENSE), and it is free to use and distribute for any purpose.

See the [License](LICENSE) file for more information.

## Authors

The project is developed and maintained by the [Deveel](https://deveel.com) team, and it is released as an open-source project to the community.