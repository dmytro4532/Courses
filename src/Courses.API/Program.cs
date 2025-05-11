using Courses.API.Apis;
using Courses.API.Extensions;
using Courses.Application;
using Courses.Infrastructure;
using Courses.Infrastructure.Extensions.DI;
using Microsoft.AspNetCore.Mvc;

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
app.EnsureBucketExistsAsync().Wait();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapCoursesApi();
app.MapUsersApi();
app.MapTopicsApi();
app.MapTestsApi();
app.MapQuestionsApi();

app.Run();
