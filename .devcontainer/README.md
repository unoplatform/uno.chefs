# Uno Chefs dev container

A Linux dev container for working on Uno Chefs: builds and runs the **WebAssembly**
and **Desktop (Skia)** heads, compiles the **Android** head, runs **Chefs.Api**, and
orchestrates the lot through an **Aspire** AppHost.

> **Host requirement: Linux or WSL2.** The container bind-mounts `/tmp/.X11-unix`
> and `$HOME/.local/share/Uno Platform` from the host, and `$HOME` is unset on
> Windows-native Docker. Run VS Code from inside your WSL distro (`code .` in the
> repo folder on the WSL side), not from Windows.

## What's inside

| | |
|---|---|
| Base | `mcr.microsoft.com/dotnet/sdk:10.0` (Ubuntu 24.04) — matches the repo's `net10.0-*` TFMs |
| Workloads | `wasm-tools`, `android` (+ Microsoft OpenJDK 21, Android SDK via `uno-check`) |
| Uno desktop deps | GTK3, WebKit2GTK, LibVLC (`MediaPlayerElement`), fontconfig (SkiaSharp), Xvfb |
| UI tests | Chrome for Testing (browser + matching chromedriver), amd64 only |
| Agents | Claude Code, GitHub Copilot CLI, `uno` + `uno-app` + `microsoft-learn` MCP servers |
| Network | dnsmasq DNS allowlist (`init-firewall.sh`) |

> .NET 10 images are **Ubuntu**-based; .NET 9's were Debian. That is why the
> browser is Chrome for Testing rather than the distro `chromium` package —
> Ubuntu's is a snap shim that cannot run in a container.

## First run

1. From your WSL distro: `code /path/to/uno.chefs`
2. **Dev Containers: Reopen in Container**

`initializeCommand` runs `initialize-host.sh` on the WSL host to generate the
`~/.ssh/uno-chefs-devcontainer-ed25519` keypair used by the *WSL Host* terminal
profile. `postStartCommand` then runs `post-start.sh` inside the container
(firewall, D-Bus, MCP registration, VS Code extension pre-install), and VS Code
waits for it before activating the workspace.

Optional host environment variables, read at container start:

| Variable | Effect |
|---|---|
| `GH_TOKEN_READONLY` | Registers the GitHub MCP server. **Rejected if it carries any write scope** — use a read-only classic PAT. |
| `DISPLAY` | Where the Desktop head renders. Defaults to `:0`. |
| `TZ` | Container timezone. Defaults to `America/Los_Angeles`. |

## Running things

```bash
# Everything: Chefs.Api + dashboard, with both client heads one click away
dotnet run --project Chefs.AppHost          # dashboard at http://localhost:18888

# Just the API
dotnet run --project Chefs.Api              # http://localhost:5116, Swagger at /swagger

# Client heads directly (mock data — the default)
dotnet run --project Chefs -f net10.0-desktop
dotnet run --project Chefs -f net10.0-browserwasm
```

Ports 5116, 18888, 51480 and 5000 are forwarded to the host.

### The Aspire AppHost

`Chefs.AppHost` registers three resources:

- **`chefs-api`** — starts automatically, pinned to **port 5116**. That pin is
  load-bearing: `Chefs/App.xaml.host.cs` hardcodes `http://localhost:5116` as the
  Kiota client's base address, so a random Aspire port would leave every non-mock
  build unable to connect.
- **`chefs-wasm`** and **`chefs-desktop`** — registered but **explicit-start**, so
  they sit "Stopped" in the dashboard until you click Start. Both are built with
  `-p:UseMocks=false` so they talk to `chefs-api` instead of the bundled JSON.

Mocking is a *compile-time* switch (`UseMocks` defines `USE_MOCKS`, which swaps
`MockHttpMessageHandler` into the Kiota client), so switching between mock and
live data is a rebuild — which is exactly what clicking Start does.

There are **no container resources**, so this needs neither Docker-in-Docker nor
`--privileged`. DCP logs a `Could not harvest all abandoned containers` warning at
startup when no Docker daemon is present; it is harmless.

### WebAssembly UI tests

Chrome for Testing is installed under `/opt` (browser and chromedriver built from
the same revision, so they cannot drift) and `UNO_UITEST_DRIVERPATH_CHROME` /
`UNO_UITEST_CHROME_BINARY_PATH` are already set. The per-run variables are not:

```bash
dotnet publish Chefs/Chefs.csproj -c Release \
  -p:TargetFrameworkOverride=net10.0-browserwasm \
  -p:IsUiAutomationMappingEnabled=True

dotnet tool install -g dotnet-serve
dotnet serve -p 5000 -d Chefs/bin/Release/net10.0-browserwasm/publish/wwwroot/ &

UNO_UITEST_PLATFORM=Browser \
UNO_UITEST_TARGETURI=http://localhost:5000 \
  dotnet test Chefs.UITests/Chefs.UITests.csproj
```

`Chefs.UITests/TestBase.cs` runs Chrome **headed** under `#if DEBUG`; use
`-c Release` for the test project, or expect it to need a display.

## What this container deliberately does not do

- **No Android emulator.** No `/dev/kvm`, no `--privileged`, no emulator system
  images. `-f net10.0-android` compiles; deploying needs a device or emulator on
  the host — use the *WSL Host* terminal profile to reach it.
- **No iOS or Windows heads.** `net10.0-ios` needs macOS + Xcode;
  `net10.0-windows10.0.19041` needs Windows. Same escape hatch.
- **No Docker.** Nothing in this repo's Aspire graph needs it.
- **No WASM UI tests on arm64.** Chrome for Testing ships no arm64 Linux build,
  so that layer is skipped on Apple Silicon; everything else still works.

## The DNS allowlist

`init-firewall.sh` points `/etc/resolv.conf` at a local dnsmasq that only resolves
an allowlist (NuGet, GitHub, Uno, Microsoft, Anthropic, npm, Android SDK CDNs).
Everything else is `REFUSED`. This is a DNS-layer filter, not an IP-layer
firewall — direct-IP egress is not blocked. Loopback is untouched, so Aspire and
the dev servers are unaffected.

Both agent CLIs are aliased into unattended mode (`claude
--dangerously-skip-permissions`, `copilot --autopilot --allow-all`); the allowlist
is what makes that a contained decision rather than an open one.

**A new dependency host is a firewall change.** Symptom: `NU1301`, `ENOTFOUND`, or
a hung restore. Add a `server=/<domain>/${UPSTREAM_DNS}` line and rebuild.

## Trimming this down

Each of these is self-contained if you want it gone:

- **Copilot CLI** — the `gh.io/copilot-install` layer in `Dockerfile`, the
  `copilot` aliases and Copilot MCP block in `post-start.sh`, the `github.copilot*`
  extensions in `devcontainer.json`, and the `githubcopilot.com` /
  certificate-revocation entries in `init-firewall.sh`.
- **WSL host terminal** — `ssh-wsl-host.sh`, `initialize-host.sh`, the
  `initializeCommand`, the SSH key mount, and the *WSL Host* terminal profile.
  Requires an sshd on the WSL host listening on port 2222; without it the profile
  simply fails to connect and nothing else is affected.
- **Firewall** — `init-firewall.sh`, its `sudo` line in `post-start.sh`, the
  `NET_ADMIN`/`NET_RAW` caps, and the `dnsmasq`/`iptables`/`dnsutils` packages.
- **Android** — the JDK layer, `android` in `dotnet workload install`, the
  `uno-check` Android SDK provisioning, and the `dl.google.com` /
  `developer.android.com` / `maven.google.com` allowlist entries.
