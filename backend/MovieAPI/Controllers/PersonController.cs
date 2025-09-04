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
        if (_dataLoader.PersonNameToMovies.TryGetValue(name, out var person))
        {
            return Ok(new
            {
                person.Name,
                ActedMovies = person.Movies[0].Select(m => m.Title).ToList(),
                DirectedMovies = person.Movies[1].Select(m => m.Title).ToList()
            });
        }
        return NotFound();
    }
}