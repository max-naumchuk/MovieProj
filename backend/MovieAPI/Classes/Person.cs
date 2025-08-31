using System.Collections.Generic;

public class Person
{
    public string Name { get; set; }
    public List<HashSet<Movie>> InMovies { get; set; } = new List<HashSet<Movie>> { new HashSet<Movie>(), new HashSet<Movie>() };
}