using FastEndpoints;
using FastEndpoints.Swagger;
using Planner.Api.Extensions;
using Planner.Application;
using Planner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseCustomAuthentication();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
});

app.UseHttpsRedirection();

app.Run();