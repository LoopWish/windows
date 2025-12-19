# GitHub Copilot Agent Instructions (Windows)

This repository contains the Loopwish Windows client.

## Organization-wide standards (Loopwish)

- Keep PRs small and reviewable; avoid drive-by refactors.
- Do not commit secrets (API keys, tokens, credentials) or `.env` files.
- Prefer secure-by-default changes; avoid logging sensitive data.
- Update docs and tests when behavior changes.
- Keep CI green and run the repo’s validation commands before opening a PR.
- Follow the existing code style and architecture; copy patterns already used in the repo.

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
