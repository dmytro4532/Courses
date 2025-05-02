using Courses.API.Apis;
using Courses.API.Extensions;
using Courses.Application;
using Courses.Infrastructure;
using Courses.Infrastructure.Extensions.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => 
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyMigrations();

app.UseCors();

app.UseHttpsRedirection();

app.MapArticlesApi();

app.Run();
