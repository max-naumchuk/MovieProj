using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/movie")]
public class MovieController : ControllerBase
{
    private readonly DataLoader _dataLoader;

    public MovieController(DataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    [HttpGet("{title}")]
    public IActionResult Get(string title)
    {
        if (_dataLoader.MovieToMovieName.TryGetValue(title, out var movie))
        {
            return Ok(new
            {
                title,
                Actors = movie.Actors.Select(a => a.Name).ToList(),
                Director = movie.Director?.Name,
                Tags = movie.Tags,
                Rating = movie.Rating
            });
        }
        return NotFound();
    }
}