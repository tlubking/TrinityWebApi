namespace TrinityWebApi.Configuration
{
    public sealed class ScriptureApiOptions
    {
        public string BaseUrl { get; init; } = "https://api.scripture.api.bible/v1/";
        public string ApiKey { get; init; } = string.Empty; // moved to user-secrets
    }
}