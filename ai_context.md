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

The use case pattern is implemented in the `Application/UseCases` directory. Each use case has its own folder containing an interface and an implementation.

-   `Application/UseCases/GetItems`: Example of a use case to retrieve items.
-   `Application/UseCases/CreateItem`: Example of a use case to create an item.

## How to Contribute

When adding new features, please follow the existing structure. For example, new controllers should be added to the `Controllers` directory. If you are adding business logic, consider creating a new folder for services or use cases.
