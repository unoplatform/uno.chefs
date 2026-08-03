// Aspire AppHost for Uno Chefs.
//
// `dotnet run --project Chefs.AppHost` brings up Chefs.Api and puts the Uno
// client apps one click away in the dashboard, so the whole "app talking to the
// real API" loop is a single command instead of two terminals plus a rebuild.
//
// Everything here is local-dev orchestration — there are no container resources,
// which is why the devcontainer needs neither Docker-in-Docker nor --privileged.

var builder = DistributedApplication.CreateBuilder(args);

// ─── Chefs.Api ────────────────────────────────────────────────────────────────
// Port 5116 is pinned, not incidental: Chefs/App.xaml.host.cs hardcodes
//   new EndpointOptions { Url = "http://localhost:5116" }
// as the Kiota client base address. Without the pin Aspire allocates a fresh
// proxy port per run and every non-mock client build silently fails to connect.
// The value matches the "http" profile in Chefs.Api/Properties/launchSettings.json
// so `dotnet run --project Chefs.Api` and the AppHost expose the same address.
//
// The health probe uses /api/Recipe/categories because the API has no dedicated
// health endpoint and categories.json is by far the smallest embedded payload
// (~1.4KB vs ~140KB for Recipes.json), so polling it is cheap.
var api = builder.AddProject<Projects.Chefs_Api>("chefs-api")
	.WithEndpoint("http", endpoint => endpoint.Port = 5116)
	.WithHttpHealthCheck("/api/Recipe/categories", endpointName: "http");

// ─── Uno client apps (registered always, started on demand) ───────────────────
// WithExplicitStart keeps these "Stopped" until you click Start in the
// dashboard: a cold Uno build is expensive and you rarely want both heads at
// once, but having them listed means one AppHost session can drive either.
//
// Two MSBuild properties matter on every one of these:
//
//   TargetFrameworkOverride — Chefs.csproj cross-targets five TFMs and `-f`
//     alone still restores all of them, which demands every workload be
//     installed. The csproj collapses TargetFrameworks to this value.
//
//   UseMocks=false — mocking is a compile-time switch (it defines USE_MOCKS,
//     which swaps MockHttpMessageHandler into the Kiota client). Running
//     against chefs-api therefore requires a rebuild, not just a config change.

// Browser-hosted, so its calls to http://localhost:5116 are cross-origin. The API
// enables a permissive CORS policy in Development for exactly this reason (see
// Chefs.Api/Program.cs) — without it every request fails preflight.
builder.AddExecutable(
		name: "chefs-wasm",
		command: "dotnet",
		workingDirectory: "..",
		args:
		[
			"run",
			"--project", "Chefs/Chefs.csproj",
			"--framework", "net10.0-browserwasm",
			"--property:TargetFrameworkOverride=net10.0-browserwasm",
			"--property:UseMocks=false",
			"--launch-profile", "Chefs (WebAssembly)",
		])
	// isProxied: false — the Uno WASM dev server owns this port via the
	// "Chefs (WebAssembly)" launch profile (applicationUrl http://localhost:51480).
	// Aspire only advertises the URL in the dashboard; proxying it would put a
	// second listener in front of a port the dev server already binds.
	.WithHttpEndpoint(port: 51480, name: "http", isProxied: false)
	.WithExternalHttpEndpoints()
	.WithExplicitStart()
	.WaitFor(api);

builder.AddExecutable(
		name: "chefs-desktop",
		command: "dotnet",
		workingDirectory: "..",
		args:
		[
			"run",
			"--project", "Chefs/Chefs.csproj",
			"--framework", "net10.0-desktop",
			"--property:TargetFrameworkOverride=net10.0-desktop",
			"--property:UseMocks=false",
		])
	// Skia desktop needs an X server. In the devcontainer DISPLAY is set
	// container-wide and /tmp/.X11-unix is bind-mounted from the WSL host, so the
	// child process inherits a working display. Outside a container this is
	// simply whatever the developer's session already has.
	.WithExplicitStart()
	.WaitFor(api);

builder.Build().Run();
