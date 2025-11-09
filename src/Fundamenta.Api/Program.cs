using FluentValidation;
using Fundamenta.Api.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.ConfigureJsonSerializer();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

app.AddScalar();
app.UseHttpsRedirection();
app.Run();
