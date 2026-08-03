# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Uno Chefs is a cross-platform recipe app that serves as Uno Platform's flagship real-world sample. Nearly every feature exists to demonstrate an Uno capability, and most are documented as a "recipe" under `doc/` that gets published into the Uno Platform docs site — changing a feature usually means updating its recipe too.

Four projects in `Chefs.sln`:

- `Chefs/` — the Uno single-project app (Android, iOS, Windows/WinAppSDK, Desktop, WebAssembly)
- `Chefs.Api/` — ASP.NET Core Web API backing the app, serving the shared `AppData/*.json`
- `Chefs.AppHost/` — Aspire orchestration for local dev (API + client heads)
- `Chefs.UITests/` — NUnit + Uno.UITest UI automation (there are no unit tests)

No CI job builds `Chefs.sln`; every pipeline targets `Chefs/Chefs.csproj` directly.

## Build & run

The Uno.Sdk version is pinned in `global.json`. Packages use Central Package Management (`Directory.Packages.props`), but most dependencies come from `<UnoFeatures>` in `Chefs/Chefs.csproj` rather than explicit `PackageReference`s — add capabilities there first.

### Always constrain the target framework

`Chefs.csproj` cross-targets five TFMs. Passing `-f`/`-p:TargetFramework` alone still *restores* all of them, forcing every workload to be installed. Pass `TargetFrameworkOverride` as a global property instead:

```bash
dotnet build Chefs/Chefs.csproj -p:TargetFrameworkOverride=net9.0-desktop
dotnet run --project Chefs -f net9.0-desktop
```

Valid values: `net9.0-desktop`, `net9.0-browserwasm`, `net9.0-windows10.0.19041`, `net9.0-android`, `net9.0-ios`.

For IDE work, copy `crosstargeting_override.props.sample` to `crosstargeting_override.props` (imported by `Directory.Build.props`) and uncomment only the platforms you need.

### Mock data vs. live API

`UseMocks` defaults to `true`, which defines `USE_MOCKS` and swaps `MockHttpMessageHandler` into the Kiota client so the app reads `AppData/*.json` locally. **Mocking is a compile-time switch, so moving to live data is a rebuild, not a config change.** To run against the real API:

```bash
dotnet run --project Chefs.Api      # http://localhost:5116, Swagger UI at /swagger
dotnet build Chefs/Chefs.csproj -p:TargetFrameworkOverride=net9.0-desktop -p:UseMocks=false
```

The endpoint URL is hardcoded in `App.xaml.host.cs` — which is why anything launching the API must pin port 5116.

### Aspire (`Chefs.AppHost`)

`dotnet run --project Chefs.AppHost` starts `Chefs.Api` on the pinned port plus the dashboard on `http://localhost:18888`, and registers `chefs-wasm` / `chefs-desktop` as **explicit-start** resources — they stay stopped until clicked, then build with `-p:UseMocks=false` against the live API.

Aspire is 9.x (not 13.x): 13.x AppHosts target `net10.0` and this repo is on .NET 9. The `Aspire.AppHost.Sdk` version lives in `Chefs.AppHost.csproj` because MSBuild SDK references are outside Central Package Management, while `Aspire.Hosting.AppHost` is in `Directory.Packages.props` — bump both together.

There are no container resources, so no Docker is required; DCP's `Could not harvest all abandoned containers` startup warning is harmless. `Chefs.Api` enables a permissive CORS policy **in Development only**, because the WebAssembly head calls it cross-origin (`:51480` → `:5116`).

### Rendering variants

CI builds each platform twice, once with `-p:UseSkiaRendering=true` and once with native rendering. If a bug is rendering-specific, reproduce with the matching flag.

### Dev container

`.devcontainer/` provides a Linux container for WASM + Desktop + Android-build work — see `.devcontainer/README.md`. **WSL2/Linux hosts only** (it bind-mounts `$HOME` paths and the X11 socket). It deliberately has no Android emulator, no iOS/Windows heads and no Docker; the *WSL Host* terminal profile is the escape hatch to the host for those.

Network egress inside the container goes through a dnsmasq **DNS allowlist** (`init-firewall.sh`). A new dependency host means a new `server=/<domain>/` line — the symptom is `NU1301`/`ENOTFOUND`/a hung restore, not an obvious network error.

### Formatting (verified in CI, so run before pushing)

```bash
dotnet format Chefs.sln
xstyler --recursive --config xaml-styler.json --directory Chefs
```

CI runs `xstyler ... --passive` to verify XAML, and commit messages must follow Conventional Commits.

## Tests

`Chefs.UITests` drives the *built* app — a published WASM site served over HTTP and driven by Selenium, or a deployed Android/iOS app. It needs the app built with `-p:IsUiAutomationMappingEnabled=True`, which also defines `USE_UITESTS` and enables the `App.GetCurrentPage()` backdoor the tests assert against. Debug builds set this automatically.

Configuration comes entirely from `UNO_UITEST_*` environment variables — `UNO_UITEST_PLATFORM` (`Browser`/`Android`/`iOS`), `UNO_UITEST_TARGETURI`, `UNO_UITEST_SCREENSHOT_PATH`, etc. See `build/scripts/*-uitest-run.sh` for the full set CI uses; those scripts are the fastest way to reproduce a CI test run locally.

```bash
dotnet test Chefs.UITests/Chefs.UITests.csproj --filter "Name~When_SmokeTest"
```

Test classes are named `Given_<Subject>`, tests `When_<Scenario>`, derived from `TestBase` and marked `[AutoRetry]`. `TestBase` screenshots on teardown and dumps browser logs.

## Architecture

Data flows one direction through four layers inside `Chefs/`: `Client/` → `Business/` → `Presentation/` → `Views/`.

- **`Client/`** — Kiota-generated API client (`ChefsApiClient`, `Client/Api/**`, `Client/Models/*Data.cs`). Generated code, marked `<auto-generated/>`; regenerate rather than hand-edit. `Client/Mock/` holds the offline handler, which dispatches by URL path to per-entity endpoint classes that load JSON from `ms-appx:///AppData/`.
- **`Business/`** — domain records (`Recipe`, `Cookbook`, `User`, …) that wrap the `*Data` DTOs through an `internal` constructor and convert back with `ToData()`, plus the services (`IRecipeService`, `ICookbookService`, `IUserService`, `INotificationService`, `IShareService`), all registered as singletons.
- **`Presentation/`** — MVUX models, one `public partial record <Name>Model` per page. Dependencies are constructor-injected; state is exposed as `IFeed`/`IState`/`IListFeed`/`IListState`; commands are plain `public async ValueTask Foo(CancellationToken ct)` methods that the MVUX source generator surfaces to XAML as bindable commands.
- **`Views/`** — `<Name>Page.xaml` per model, plus `Controls/`, `Dialogs/`, `Flyouts/`, `Templates/`. Resource dictionaries live in `Styles/`, value converters in `Converters/`.

### Composition root

`Chefs/App.xaml.host.cs` wires everything: authentication, the Kiota client (and mock handler), logging, configuration sections, serialization, services, and `RegisterRoutes` — which declares every `ViewMap`/`DataViewMap` and the nested `RouteMap` tree. **Adding a page means adding a model, a page, and both registrations here.**

### Cross-cutting patterns

- **Messaging** — services publish `EntityMessage<T>` via `IMessenger` (`WeakReferenceMessenger`); models subscribe with `.Observe(_messenger, x => x.Id)`, so a change made on one page updates every other list holding that entity. This is why most list state is created with `.Observe(...)` rather than plain `ListFeed.Async`.
- **Navigation** — routes are strings resolved against the `RouteMap` tree (e.g. `"/Main/-/Search"`), issued from XAML via `uen:Navigation.Request` or from a model via `_navigator.NavigateRouteAsync`. Address-bar updates are disabled and the launch URL is cleared at startup (deep-linking issue #738).
- **Serialization** — all JSON goes through source-generated `JsonSerializerContext`s (`MockEndpointContext`, `AppConfigContext`). New mock payload types must be registered in `ConfigureSerialization`, or they work in Debug and fail under AOT/WASM trimming.
- **Shared data** — root-level `AppData/*.json` feeds both apps: `Content` linked into `ms-appx:///AppData/` for `Chefs`, `EmbeddedResource` for `Chefs.Api`.

### XAML conventions

Prefixes used throughout: `utu:` Uno.Toolkit.UI, `uer:` Uno.Extensions.Reactive.UI (`FeedView`), `uen:` Uno.Extensions.Navigation.UI, `muxc:` Microsoft.UI.Xaml.Controls, `ut:` Uno.Themes. Platform-conditional markup uses the `mobile` / `not_mobile` XAML namespaces, resolved by the `IncludeXamlNamespaces`/`ExcludeXamlNamespaces` items in `Chefs.csproj`.

`FEATURES.md` catalogues every control and helper the app uses, with a one-line rationale for each.

## Code style

`.editorconfig` is authoritative and enforced by `dotnet format` in CI. Notably: **tabs** in C# and XAML, file-scoped namespaces (warning), expression-bodied members when they fit on one line (warning), nullable and implicit usings enabled. `Chefs/GlobalUsings.cs` is large — check it before adding a `using`.

## Documentation (`doc/`)

The Recipe Book is one Markdown file per feature, each with DocFx `uid:` front matter and cross-referenced as `xref:Uno.Recipes.<Name>`. A new recipe must be listed in **both** `doc/RecipeBooksOverview.md` and `doc/toc.yml`. Recipes link to source using permalinked GitHub URLs (commit SHA plus line range), so moving code silently invalidates them.

`doc/docs-setup-local.md` explains how to render the Recipe Book locally against a clone of the main Uno repo.
