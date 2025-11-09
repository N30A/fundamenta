using Fundamenta.Api.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

app.AddScalar();
app.UseHttpsRedirection();
app.Run();
