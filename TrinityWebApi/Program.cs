using Scalar.AspNetCore;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Bind to Railway PORT if provided (useful when not using Dockerfile)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddOptions<TrinityWebApi.Configuration.ScriptureApiOptions>()
    .Bind(builder.Configuration.GetSection("ScriptureApi"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "ScriptureApi:ApiKey is required.")
    .ValidateOnStart();

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

const string CorsPolicy = "Cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            var csv = builder.Configuration["Cors:AllowedOrigins"];
            var origins = (csv ?? string.Empty).Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (origins.Length > 0)
                policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

builder.Services.AddHttpClient("ScriptureApi", (sp, client) =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<TrinityWebApi.Configuration.ScriptureApiOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(opts.BaseUrl) ? "https://api.scripture.api.bible/v1/" :
                  (opts.BaseUrl.EndsWith("/") ? opts.BaseUrl : opts.BaseUrl + "/");

    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("api-key", opts.ApiKey);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(15);
})
.AddStandardResilienceHandler(options =>
{
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(20);
    options.Retry.MaxRetryAttempts = 3;
});

var app = builder.Build();
app.Logger.LogInformation("BaseAddress: {Base}, AllowedOrigins: {Cors}, Env: {Env}",
    builder.Configuration["ScriptureApi:BaseUrl"],
    builder.Configuration["Cors:AllowedOrigins"],
    app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.UseAuthorization();

// Simple health endpoint for Railway probe
app.MapGet("/", () => Results.Ok("OK"));

app.MapControllers();
app.Run();
