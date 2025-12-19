---
name: loopwish-windows-dotnet
description: Workflows for the Loopwish Windows client (.NET 8 WPF): build/test with dotnet CLI, keep Core vs App boundaries, and fix CI failures.
---

# Loopwish Windows (.NET/WPF) Skill

Use this skill when working in the Loopwish **windows repository**.

## When to use

- Implementing or fixing WPF UI.
- Updating shared/domain logic in `Loopwish/Core`.
- Fixing CI failures on `windows-latest`.

## Key structure

- Solution: `Loopwish.sln`
- Core project: `Loopwish/Core/Loopwish.Core.csproj`
- App project: `Loopwish/App/Loopwish.App.csproj`
- Tests: `Loopwish/Tests/Loopwish.Tests.csproj`

## Validation (matches CI)

From the repo root:

```bash
dotnet restore

dotnet build -c Release --no-restore

dotnet test -c Release --no-build
```

## Typical workflow for a change

1. Keep domain/common logic in `Loopwish.Core`.
2. Keep UI logic in `Loopwish.App`; avoid growing business logic in code-behind.
3. Update/add xUnit tests in `Loopwish.Tests` when behavior changes.

## Example prompts

- “Add a small service class to `Loopwish.Core` and unit test it.”
- “Fix failing tests in CI without widening scope.”
