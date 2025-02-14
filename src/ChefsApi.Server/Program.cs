using Uno.Wasm.Bootstrap.Server;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure the RouteOptions to use lowercase URLs
    builder.Services.Configure<RouteOptions>(options =>
        options.LowercaseUrls = true);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Include XML comments for all included assemblies
        Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml")
            .Where(x => x.Contains("ChefsApi")
                && File.Exists(Path.Combine(
                    AppContext.BaseDirectory,
                    $"{Path.GetFileNameWithoutExtension(x)}.dll")))
            .ToList()
            .ForEach(path => c.IncludeXmlComments(path));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseUnoFrameworkFiles();
    app.MapFallbackToFile("index.html");

    app.UseStaticFiles();
    
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine("Application terminated unexpectedly");
    Console.Error.WriteLine(ex);
#if DEBUG
    if (System.Diagnostics.Debugger.IsAttached)
    {
        System.Diagnostics.Debugger.Break();
    }
#endif
}
