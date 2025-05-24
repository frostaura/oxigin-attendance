// Program entry point for Oxigin Attendance API.
// Configures services, middleware, and starts the web application.
using Oxigin.Attendance.Core.Extensions;
using Oxigin.Attendance.Datastore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCoreServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Development-only bootstrapping goes here.
}

app.UseSwagger(); // Enable Swagger for API documentation
app.UseSwaggerUI(); // Enable Swagger UI
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseAuthorization(); // Enable authorization middleware
app.MapControllers(); // Map controller routes
app.UseDataResources<Program>(); // Custom extension for data resources
app.Run(); // Start the application
