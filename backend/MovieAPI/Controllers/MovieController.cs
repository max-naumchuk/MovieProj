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
        if (_dataLoader.MoviesByMovieName.TryGetValue(title, out var movie))
        {
            return Ok(new
            {
                movie.Title,
                Actors = movie.Actors,
                movie.Director,
                Tags = movie.Tags,
                movie.Rating
            });
        }
        return NotFound();
    }
}