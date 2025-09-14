using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/person")]
public class PersonController : ControllerBase
{
    private readonly DataLoader _dataLoader;

    public PersonController(DataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    [HttpGet("{name}")]
    public IActionResult Get(string name)
    {
        if (_dataLoader.PersonNameToMovies.TryGetValue(name, out var movies))
        {
            var actedMovies = movies[0].Select(m => m.Title).ToList();
            var directedMovies = movies[1].Select(m => m.Title).ToList();

            return Ok(new
            {
                name,
                acted_movies = actedMovies,
                directed_movies = directedMovies
            });
        }
        return NotFound();
    }
}