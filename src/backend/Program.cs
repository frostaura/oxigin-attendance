using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services for client request management, staff allocation, employee features, reminders, notifications, and administrative features
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISiteManagerService, SiteManagerService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITimesheetService, TimesheetService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

// Add middleware for handling SMS/Email notifications, geo-location, facial recognition, and timesheet generation
app.UseMiddleware<SmsNotificationMiddleware>();
app.UseMiddleware<EmailNotificationMiddleware>();
app.UseMiddleware<GeoLocationMiddleware>();
app.UseMiddleware<FacialRecognitionMiddleware>();
app.UseMiddleware<TimesheetGenerationMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/api/data", async context =>
    {
        await context.Response.WriteAsJsonAsync(new { message = "Hello from the backend!" });
    });
});

app.Run();
