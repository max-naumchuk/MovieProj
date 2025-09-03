using System.Collections.Generic;

public class Person
{
    public string Name { get; set; }
    public List<HashSet<string>> InMovies { get; set; } = new List<HashSet<string>> { new HashSet<string>(), new HashSet<string>() };
}