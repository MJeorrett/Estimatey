using Estimatey.Application;
using Estimatey.Infrastructure;
using Estimatey.Infrastructure.Float;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

#region addServices

services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()));

services.AddControllers();
services.AddHttpClient();
services.AddHttpContextAccessor();

services.AddApplication(configuration);
services.AddInfrastructure(configuration);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region configurePipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var hangfireLogin = configuration
    .GetSection("HangfireCredentials")
    .GetValue<string>("Username");

var hangfirePassword = configuration
    .GetSection("HangfireCredentials")
    .GetValue<string>("Password");

app.UseHangfireDashboard(options: new()
{
    Authorization = new[]
    {
        new BasicAuthAuthorizationFilter(new()
        {
            LoginCaseSensitive = true,
            Users = new[]
            {
                new BasicAuthAuthorizationUser
                {
                    Login = hangfireLogin,
                    PasswordClear = hangfirePassword,
                }
            }
        })
    },
    AppPath = "/swagger",
    DashboardTitle = "Estimatey"
});

RecurringJob.AddOrUpdate(
    "Reports Snapshot",
    () => app.Services.CreateScope()
        .ServiceProvider.GetRequiredService<FloatHistoricLoggedTimeSyncer>()
        .SyncHistoricLoggedTime(),
    Cron.Daily);

// Run once on startup
BackgroundJob.Enqueue(
    () => app.Services.CreateScope()
        .ServiceProvider.GetRequiredService<FloatHistoricLoggedTimeSyncer>()
        .SyncHistoricLoggedTime());

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion

public partial class Program { }