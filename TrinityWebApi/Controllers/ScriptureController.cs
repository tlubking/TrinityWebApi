using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace TrinityWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScriptureController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ScriptureController> _logger;

        public ScriptureController(IHttpClientFactory httpClientFactory, ILogger<ScriptureController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET /api/scripture/bibles
        [HttpGet("bibles")]
        public async Task<IActionResult> GetBibles(CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles"); 
            var upstream = await client.GetAsync("bibles", ct);

            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/books
        [HttpGet("bibles/{bibleId}/books")]
        public async Task<IActionResult> GetBooks(string bibleId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/books");
            var upstream = await client.GetAsync($"bibles/{bibleId}/books", ct);

            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/books/{bookId}
        [HttpGet("bibles/{bibleId}/books/{bookId}")]
        public async Task<IActionResult> GetBookById(string bibleId, string bookId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/books/{bookId}");
            var path = $"bibles/{bibleId}/books/{bookId}";
            var upstream = await client.GetAsync(path, ct);

            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/books/{bookId}/chapters
        [HttpGet("bibles/{bibleId}/books/{bookId}/chapters")]
        public async Task<IActionResult> GetChapters(string bibleId, string bookId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/books/{bookId}/chapters");
            var path = $"bibles/{bibleId}/books/{bookId}/chapters";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/chapters/{chapterId}
        [HttpGet("bibles/{bibleId}/chapters/{chapterId}")]
        public async Task<IActionResult> GetChapterById(string bibleId, string chapterId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/chapters/{chapterId}");
            var path = $"bibles/{bibleId}/chapters/{chapterId}";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/books/{bookId}/sections
        [HttpGet("bibles/{bibleId}/books/{bookId}/sections")]
        public async Task<IActionResult> GetSections(string bibleId, string bookId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/books/{bookId}/sections");
            var path = $"bibles/{bibleId}/books/{bookId}/sections";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/chapters/{chapterId}/sections
        [HttpGet("bibles/{bibleId}/chapters/{chapterId}/sections")]
        public async Task<IActionResult> GetSectionsByChapterId(string bibleId, string chapterId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/chapters/{chapterId}/sections");
            var path = $"bibles/{bibleId}/chapters/{chapterId}/sections";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/sections/{sectionId}
        [HttpGet("bibles/{bibleId}/sections/{sectionId}")]
        public async Task<IActionResult> GetSectionById(string bibleId, string sectionId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/sections/{sectionId}");
            var path = $"bibles/{bibleId}/sections/{sectionId}";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/passages/{passageId}
        [HttpGet("bibles/{bibleId}/passages/{passageId}")]
        public async Task<IActionResult> GetPassageById(string bibleId, string passageId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/passages/{passageId}");
            var path = $"bibles/{bibleId}/passages/{passageId}";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/chapters/{chapterId}/verses
        [HttpGet("bibles/{bibleId}/chapters/{chapterId}/verses")]
        public async Task<IActionResult> GetVersesByChapterId(string bibleId, string chapterId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/chapters/{chapterId}/verses");
            var path = $"bibles/{bibleId}/chapters/{chapterId}/verses";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/verses/{verseId}
        [HttpGet("bibles/{bibleId}/verses/{verseId}")]
        public async Task<IActionResult> GetVerseById(string bibleId, string verseId, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/verses/{verseId}");
            var path = $"bibles/{bibleId}/verses/{verseId}";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }

        // GET /api/scripture/bibles/{bibleId}/search?query=...
        [HttpGet("bibles/{bibleId}/search")]
        public async Task<IActionResult> SearchInBible(string bibleId, [FromQuery] string query, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("ScriptureApi");
            _logger.LogInformation("Calling Scripture API {Url}", $"bibles/{bibleId}/search?query={Uri.EscapeDataString(query)}");
            var path = $"bibles/{bibleId}/search?query={Uri.EscapeDataString(query)}";
            var upstream = await client.GetAsync(path, ct);
            var json = await upstream.Content.ReadAsStringAsync(ct);
            return new ContentResult
            {
                StatusCode = (int)upstream.StatusCode,
                Content = json,
                ContentType = MediaTypeNames.Application.Json
            };
        }
    }
}