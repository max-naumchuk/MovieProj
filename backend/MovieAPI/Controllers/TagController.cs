using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/tag")]
public class TagController : ControllerBase
{
    private readonly DataLoader _dataLoader;

    public TagController(DataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    [HttpGet("{tag}")]
    public IActionResult Get(string tag)
    {
        if (_dataLoader.TagsToMovies.TryGetValue(tag, out var movies))
        {
            return Ok(movies.Select(m => m.Title).ToList());
        }
        return NotFound();
    }
}