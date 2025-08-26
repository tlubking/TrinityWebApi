using Scalar.AspNetCore;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http.Resilience;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Bind options
builder.Services
    .AddOptions<TrinityWebApi.Configuration.ScriptureApiOptions>()
    .Bind(builder.Configuration.GetSection("ScriptureApi"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "ScriptureApi:ApiKey is required.")
    .ValidateOnStart();

// CORS
const string CorsPolicyName = "Cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            var originsCsv = builder.Configuration["Cors:AllowedOrigins"]; // e.g. "https://app.example.com,https://www.example.com"
            var origins = (originsCsv ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (origins.Length == 0)
            {
                // Fail closed by default in prod
                policy.WithOrigins(Array.Empty<string>()).AllowAnyHeader().AllowAnyMethod();
            }
            else
            {
                policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
            }
        }
    });
});

// Typed HttpClient
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
    options.Retry.MaxRetryAttempts = 3; // retry 5xx/408/429 with jittered backoff
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();            // /openapi/v1.json
    app.MapScalarApiReference(); // /scalar
}

app.UseHttpsRedirection();
app.UseCors(CorsPolicyName);
app.UseAuthorization();
app.MapControllers();
app.Run();
