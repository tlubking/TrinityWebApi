using Scalar.AspNetCore;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http.Resilience;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddOptions<TrinityWebApi.Configuration.ScriptureApiOptions>()
    .Bind(builder.Configuration.GetSection("ScriptureApi"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "ScriptureApi:ApiKey is required.")
    .ValidateOnStart();

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
app.UseAuthorization();
app.MapControllers();
app.Run();
