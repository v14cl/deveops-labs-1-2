using Microsoft.EntityFrameworkCore;
using mywebapp.Data;
using mywebapp.Endpoints;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSystemd();

builder.Configuration
    .AddJsonFile("/etc/mywebapp/config.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var appPort = builder.Configuration.GetValue<int?>("Application:Port")
              ?? builder.Configuration.GetValue<int?>("Port")
              ?? 8000;

var appHost = builder.Configuration["Application:Host"] ?? "127.0.0.1";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? builder.Configuration.GetConnectionString("PostgreSQL")
                       ?? builder.Configuration["Database:ConnectionString"]
                       ?? "Host=127.0.0.1;Port=5432;Database=mywebappdb;Username=mywebapp;Password=mywebapp";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.WebHost.ConfigureKestrel(options =>
{
    options.UseSystemd();

    if (!SystemdSocketWasProvided())
    {
        if (appHost is "0.0.0.0" or "*" or "+")
        {
            options.ListenAnyIP(appPort);
        }
        else
        {
            options.Listen(IPAddress.Parse(appHost), appPort);
        }
    }
});

var app = builder.Build();

app.MapSystemEndpoints();
app.MapNotesEndpoints();

var migrateOnly = args.Contains("--migrate", StringComparer.OrdinalIgnoreCase);
var autoMigrate = builder.Configuration.GetValue("Database:AutoMigrate", true);

if (migrateOnly || autoMigrate)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (migrateOnly)
{
    return;
}

app.Run();

static bool SystemdSocketWasProvided()
{
    return int.TryParse(Environment.GetEnvironmentVariable("LISTEN_FDS"), out var count) && count > 0;
}
