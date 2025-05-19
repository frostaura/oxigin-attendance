using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        // Configure services for client request management, staff allocation, employee features, reminders, notifications, and administrative features
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ISiteManagerService, SiteManagerService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ITimesheetService, TimesheetService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        // Configure middleware for handling SMS/Email notifications, geo-location, facial recognition, and timesheet generation
        app.UseMiddleware<SmsNotificationMiddleware>();
        app.UseMiddleware<EmailNotificationMiddleware>();
        app.UseMiddleware<GeoLocationMiddleware>();
        app.UseMiddleware<FacialRecognitionMiddleware>();
        app.UseMiddleware<TimesheetGenerationMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
