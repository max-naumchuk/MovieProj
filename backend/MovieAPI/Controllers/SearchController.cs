using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly DataLoader _dataLoader;

    public SearchController(DataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    [HttpGet]
    public IActionResult Get(string query)
    {
        if (string.IsNullOrEmpty(query) || query.Length < 3) return Ok(new List<object>());

        var suggestions = new List<object>();

        // Movies
        var movieMatches = _dataLoader.MoviesToMovieName
            .Where(kv => kv.Key.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .Select(kv => new { Type = "movie", Value = kv.Key });

        suggestions.AddRange(movieMatches);

        // Persons
        var personMatches = _dataLoader.PersonNameToMovies
            .Where(kv => kv.Key.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .Select(kv => new { Type = "person", Value = kv.Key });

        suggestions.AddRange(personMatches);

        // Tags
        var tagMatches = _dataLoader.TagsToMovies
            .Where(kv => kv.Key.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .Select(kv => new { Type = "tag", Value = kv.Key });

        suggestions.AddRange(tagMatches);

        return Ok(suggestions.Take(10));
    }
}