using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("EventDatabaseConnection")));
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings-DefaultConnection2");


// Add services to the container.
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<EventService>();

// For solving CORS problems
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
var app = builder.Build();



app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// For cors solving
app.UseCors("AllowAll");

app.Run();
