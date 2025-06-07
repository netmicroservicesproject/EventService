using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabaseConnection")));
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings-DefaultConnection2");
Console.WriteLine($"Connection is set: {connectionString != null}");
bool isConnectionStringSet = Environment.GetEnvironmentVariable("ConnectionStrings-DefaultConnection2") != null;
Console.WriteLine($"Connection string set: {isConnectionStringSet}");
connectionString = builder.Configuration.GetConnectionString("EventDatabaseConnection")
                      ?? Environment.GetEnvironmentVariable("ConnectionStrings-DefaultConnection2");
// Add services to the container.

//var keyVaultUrl = "https://your-keyvault.vault.azure.net/";
//builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
//var connectionString = builder.Configuration["ConnectionStrings-DefaultConnection2"];


builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<EventService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Enables console logging, which Azure should capture
builder.Logging.AddDebug();   // Adds debug output for local troubleshooting
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Startup");
logger.LogInformation("Application is starting up...");
// For solving CORS problems
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    try {
        var testQuery = await context.Events.FirstOrDefaultAsync();
        Console.WriteLine($"Test query successful: {testQuery != null}");
    } catch (Exception ex) {
        Console.WriteLine($"Database error: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// For cors solving
app.UseCors("AllowAll");

app.Run();
