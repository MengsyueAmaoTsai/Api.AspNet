using Hangfire;

using RichillCapital.Api.Endpoints;
using RichillCapital.Api.Middlewares;
using RichillCapital.Api.OpenApi;
using RichillCapital.Infrastructure.BackgroundJobs;
using RichillCapital.Infrastructure.Clock;
using RichillCapital.Infrastructure.Events;
using RichillCapital.Infrastructure.Logging;
using RichillCapital.Infrastructure.Persistence;
using RichillCapital.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseOptions();

builder.WebHost.UseCustomLogger();

builder.Services.AddApplicationServices();

builder.Services.AddDateTimeProvider();
builder.Services.AddDomainEventServices();
builder.Services.AddPersistence();
builder.Services.AddBackgroundJobs();

builder.Services.AddMiddlewares();
builder.Services.AddOpenApi();
builder.Services.AddEndpoints();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

app.ResetDatabase();

app.UseForwardedHeaders();

app.UseRequestDebuggingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler(options =>
{
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAnyOrigin");

app.UseSwaggerDoc();

app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = [],
    DarkModeEnabled = true,
});

app.MapEndpoints();

var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();
backgroundJobClient.Enqueue<IInstrumentInitializationJob>(job => job.ProcessAsync());

app.Services.GetRequiredService<IRecurringJobManager>()
    .AddOrUpdate<IInstrumentInitializationJob>(
        "instrument-initialization",
        job => job.ProcessAsync(),
        "0 0 * * *");

await app.RunAsync();

public partial class Program;
