using System.Collections.Generic;

public class Movie
{
    public string Title { get; set; }
    public HashSet<Person> Actors { get; set; } = new HashSet<Person>();
    public Person Director { get; set; }
    public HashSet<string> Tags { get; set; } = new HashSet<string>();
    public double Rating { get; set; }

}