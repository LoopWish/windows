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

## Git & GitHub procedure

- `main` is protected: create a feature branch, push, and open a PR.
- If updating shared vendored assets:
	- Update `.loopwish/shared.ref` to the desired shared tag (example: `v0.1.1`).
	- Ensure `vendor/shared/...` matches `shared@<tag>` exactly.
	- Update `scripts/verify_shared_vendor.ps1` if new files are added to the vendoring contract.
	- Run `powershell -ExecutionPolicy Bypass -File scripts/verify_shared_vendor.ps1` to validate vendored files match the pinned tag.
- Prefer merging shared changes/tag first, then updating this repo to pin/vendor that tag.
- After merge, clean up local branches safely:

```bash
git fetch --prune
git branch --merged origin/main
```

## Safety

- Never commit secrets or credentials.
- Avoid adding telemetry unless explicitly requested.
