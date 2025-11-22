# Contributing to SomeStuff

First off, thank you for considering contributing to this project!

## How to Contribute

When adding new features, please follow the existing structure. For example, new controllers should be added to the `Controllers` directory. If you are adding business logic, consider creating a new folder for services or use cases.

## Commit Message Guidelines

This project follows the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification. All contributors should adhere to this format when creating commit messages.

### Format

The commit message should be structured as follows:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types

-   **feat**: A new feature
-   **fix**: A bug fix
-   **docs**: Documentation only changes
-   **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
-   **refactor**: A code change that neither fixes a bug nor adds a feature
-   **perf**: A code change that improves performance
-   **test**: Adding missing tests or correcting existing tests
-   **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
-   **ci**: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
-   **chore**: Other changes that don't modify `src` or `test` files

### Examples

**Commit message with no body:**

```
feat: add new endpoint for item creation
```

**Commit message with a scope:**

```
feat(api): add new endpoint for item creation
```

**Commit message with a body:**

```
refactor: use ItemDto in ItemService

The ItemService has been updated to use the ItemDto for consistency
across the application, reducing the need for manual mapping.
```
