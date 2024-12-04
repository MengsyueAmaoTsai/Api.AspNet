using RichillCapital.Api.Endpoints;
using RichillCapital.Api.Middlewares;
using RichillCapital.Api.OpenApi;
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

builder.Services.AddMiddlewares();
builder.Services.AddOpenApi();
builder.Services.AddEndpoints();

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

app.UseSwaggerDoc();

app.MapEndpoints();


await app.RunAsync();

public partial class Program;
