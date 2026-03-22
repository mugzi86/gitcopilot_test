using WeatherApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health");

app.MapGet("/error", () => Results.Problem("An error occurred."));

app.MapGet("/weatherforecast", (WeatherService service) => service.GetWeatherForecast())
    .WithName("GetWeatherForecast");

app.Run();
