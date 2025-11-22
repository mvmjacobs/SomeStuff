# AI and Agent Context for SomeStuff Project

This document provides context for AI assistants, agents, and other automated tools to understand the purpose and structure of the `SomeStuff` project.

## Project Overview

`SomeStuff` is a .NET Web API project that will be used to code some design patterns examples, as a study project. It will contain various examples to serve as a learning resource.

## Technology Stack

*   **Language:** C#
*   **Framework:** .NET 8
*   **Project Type:** Web API

## Project Structure

The project follows a standard .NET Web API structure:

-   `Controllers/`: Contains the API controllers.
-   `Program.cs`: The main entry point of the application.
-   `appsettings.json`: Configuration file.
-   `SomeStuff.csproj`: The project file.

## Design Patterns

This project will contain examples of different design patterns.

### Use Case Pattern

The use case pattern is implemented in the `Application/UseCases` directory. Each use case has its own folder containing an interface and an implementation. This pattern now uses the `ItemDto` for data transfer.

-   `Application/UseCases/GetItems`: Example of a use case to retrieve items.
-   `Application/UseCases/CreateItem`: Example of a use case to create an item.

### DTO (Data Transfer Object) Pattern

The DTO pattern is used to transfer data between layers of the application. This helps to decouple the internal domain models from the external contracts (e.g., API responses).

-   `Application/Dtos/ItemDto.cs`: An example of a DTO used for item data.

### Mediator Pattern

The Mediator pattern is implemented using the MediatR library. The requests (queries and commands) and handlers are located in the `Application/Mediator` folder.

-   `Application/Mediator/MediatR/Queries/GetItemsQuery.cs`: A query to get a list of items.
-   `Application/Mediator/MediatR/Commands/CreateItemCommand.cs`: A command to create a new item.
-   `Application/Mediator/MediatR/Handlers/`: This directory contains the handlers for the queries and commands.
-   `Controllers/MediatorController.cs`: A controller that uses MediatR to send requests.

## How to Contribute

Please refer to the `CONTRIBUTING.md` file for guidelines on how to contribute to this project, including the commit message format.

