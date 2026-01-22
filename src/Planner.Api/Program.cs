using Planner.Api.Extensions;
using Planner.Api.Features.Projects;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Use full type name for schema IDs to avoid conflicts when multiple endpoints have nested types with same name
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Planner API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapEndpoint<GetProjects, ProjectsGroup>();
app.MapEndpoint<CreateProject, ProjectsGroup>();
app.MapEndpoint<GetProjectById, ProjectsGroup>();

app.Run();