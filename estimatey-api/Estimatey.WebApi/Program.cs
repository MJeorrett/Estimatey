using Estimatey.Application;
using Estimatey.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

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

services.AddApplication(builder.Configuration);
services.AddInfrastructure(builder.Configuration);

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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion

public partial class Program { }