using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.IdentityModel.Tokens;

public class DataLoader
{
    public Dictionary<string, Movie> MoviesByTitle { get; private set; } = new Dictionary<string, Movie>();
    public Dictionary<string, HashSet<Movie>> PersonsToMovies { get; private set; } = new Dictionary<string, HashSet<Movie>>();
    public Dictionary<string, HashSet<Movie>> TagsToMovies { get; private set; } = new Dictionary<string, HashSet<Movie>>();

    const string Path = "Data/";



    private HashSet<string> LoadMovieTConsts(string path)
    {
        var set = new HashSet<string>();
        if (!File.Exists(path)) return set;
        using var reader = new StreamReader(path);
        reader.ReadLine(); // header
        string line;
        bool english = false;
        bool russian = false;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('\t');
            if (parts.Length < 5) continue;
            string tconst = parts[0];
            int order = Convert.ToInt16(parts[1]);
            if (order == 1)
            {
                english = false;
                russian = false;
            }
            if (parts[3] == "US" | parts[3] == "GB" | parts[3] == "CA" | parts[4] == "en") english = true;
            if (parts[3] == "RU" | parts[4] == "ru") russian = true;
            if (english && russian) set.Add(tconst);
        }
        return set;
    }
}
