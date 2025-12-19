# GitHub Copilot Agent Instructions (Windows)

This repository contains the Loopwish Windows client.

## Stack

- .NET 8
- WPF
- Tests: xUnit

## Goals

- Keep the solution buildable on `windows-latest`.
- Prefer small PRs with clear descriptions.
- Respect the folder structure: `Loopwish/App`, `Loopwish/Core`, `Loopwish/Features`, `Loopwish/Tests`.

## Coding conventions

- Keep nullable enabled and avoid introducing warnings.
- Keep domain/common logic in `Loopwish.Core`.
- Keep UI logic in WPF project (`Loopwish.App`), avoid business logic in code-behind when it grows.

## Validation

Before opening a PR, run:

```bash
dotnet restore
dotnet build -c Release
dotnet test -c Release
```

## Safety

- Never commit secrets or credentials.
- Avoid adding telemetry unless explicitly requested.
