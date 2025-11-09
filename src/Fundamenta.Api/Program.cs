using FluentValidation;
using Fundamenta.Api;
using Fundamenta.Api.Host.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.ConfigureJsonSerializer();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.MapEndpoints();
app.AddScalar();
app.UseHttpsRedirection();
app.Run();
