using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.IdentityModel.Tokens;

public class DataLoader
{
    public Dictionary<string, Movie> MovieToTConst { get; private set; } = new Dictionary<string, Movie>();
    public Dictionary<string, Movie> MovieToMovieName { get; private set; } = new Dictionary<string, Movie>();
    public Dictionary<string, HashSet<Movie>> PersonNameToMovies { get; private set; } = new Dictionary<string, HashSet<Movie>>();
    public Dictionary<string, HashSet<Movie>> TagsToMovies { get; private set; } = new Dictionary<string, HashSet<Movie>>();
    public Dictionary<string, Person> PersonsToNConst { get; private set; } = new Dictionary<string, Person>();

    private void LoadData(string dataPath)
    {
        MovieToTConst = InitMovies(dataPath + "MovieCodes_IMDB.tsv");
        LoadRatings(dataPath + "Ratings_IMDB.tsv", MovieToTConst);
        LoadTags(dataPath, MovieToTConst);
        PersonsToNConst = InitPerson(dataPath + "ActorsDirectorsNames_IMDB.txt");
        LoadPersonMovies(dataPath + "ActorsDirectorsCodes_IMDB.tsv", MovieToTConst, PersonsToNConst);

        foreach (var movie in MovieToTConst) {
            MovieToMovieName.Add(movie.Value.Title, movie.Value);
        }
        foreach (var person in PersonsToNConst.Values)
        {
            foreach (var movieId in person.InMovies[0])
            {
                if (MovieToTConst.TryGetValue(movieId, out var movie))
                {
                    if (!PersonNameToMovies.ContainsKey(person.Name))
                    {
                        PersonNameToMovies[person.Name] = new HashSet<Movie>();
                    }
                    PersonNameToMovies[person.Name].Add(movie);
                }
            }
            foreach (var movieId in person.InMovies[1])
            {
                if (MovieToTConst.TryGetValue(movieId, out var movie))
                {
                    if (!PersonNameToMovies.ContainsKey(person.Name))
                    {
                        PersonNameToMovies[person.Name] = new HashSet<Movie>();
                    }
                    PersonNameToMovies[person.Name].Add(movie);
                }
            }
        }
        foreach (var movie in MovieToTConst.Values)
        {
            foreach (var tag in movie.Tags)
            {
                if (!TagsToMovies.ContainsKey(tag))
                {
                    TagsToMovies[tag] = new HashSet<Movie>();
                }
                TagsToMovies[tag].Add(movie);
            }
        }

    }

    private Dictionary<string, Movie> InitMovies(string path)
    {
        var movies = new Dictionary<string, Movie>();
        if (!File.Exists(path)) return movies;
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
            if (english && russian) movies.Add(tconst, new Movie { Title = parts[2] });
        }
        return movies;
    }
    private void LoadRatings(string path, Dictionary<string, Movie> moviesByTConst)
    {
        if (!File.Exists(path)) return;
        using var reader = new StreamReader(path);
        reader.ReadLine();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('\t');
            if (parts.Length < 3) continue;
            string tconst = parts[0];
            if (double.TryParse(parts[1], out double rating) && moviesByTConst.TryGetValue(tconst, out var movie))
            {
                movie.Rating = rating;
            }
        }
    }
    private Dictionary<string, Person> InitPerson(string path)
    {
        var personsByNConst = new Dictionary<string, Person>();
        if (!File.Exists(path)) return personsByNConst;
        using var reader = new StreamReader(path);
        reader.ReadLine();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('\t');
            if (parts.Length < 6) continue;
            string nconst = parts[0];
            string name = parts[1];
            var person = new Person { Name = name };
            personsByNConst.Add(nconst, person);
        }
        return personsByNConst;
    }

    private void LoadPersonMovies(string path, Dictionary<string, Movie> moviesByTConst, Dictionary<string, Person> personsByNConst)
    {
        if (!File.Exists(path)) return;
        using var reader = new StreamReader(path);
        reader.ReadLine();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split('\t');
            if (parts.Length < 4) continue;
            string nconst = parts[0];
            string tconst = parts[2];
            string category = parts[3];
            if (personsByNConst.TryGetValue(nconst, out var person) && moviesByTConst.TryGetValue(tconst, out var movie))
            {
                if (category == "actor" || category == "actress")
                {
                    person.InMovies[0].Add(tconst);
                    movie.Actors.Add(person);
                }
                else if (category == "director")
                {
                    person.InMovies[1].Add(tconst);
                    movie.Director = person;
                }
            }
        }

    }


    private void LoadTags(string dataPath, Dictionary<string, Movie> moviesByTConst)
    {
        string linksPath = dataPath + "links_IMDB_MovieLens.csv";
        string tagCodesPath = dataPath + "TagCodes_MovieLens.csv";
        string tagScoresPath = dataPath + "TagScores_MovieLens.csv";

        var movieIdToTConst = new Dictionary<int, string>();
        if (File.Exists(linksPath))
        {
            using var reader = new StreamReader(linksPath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length < 2 || !int.TryParse(parts[0], out int movieId) || !int.TryParse(parts[1], out int imdbId)) continue;
                string tconst = "tt" + imdbId.ToString("D7");
                movieIdToTConst[movieId] = tconst;
            }
        }

        var tagIdToTag = new Dictionary<int, string>();
        if (File.Exists(tagCodesPath))
        {
            using var reader = new StreamReader(tagCodesPath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length < 2 || !int.TryParse(parts[0], out int tagId)) continue;
                string tag = parts[1];
                tagIdToTag[tagId] = tag;
            }
        }

        if (File.Exists(tagScoresPath))
        {
            using var reader = new StreamReader(tagScoresPath);
            reader.ReadLine();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length < 3 || !int.TryParse(parts[0], out int movieId) || !int.TryParse(parts[1], out int tagId) || !double.TryParse(parts[2], out double relevance)) continue;
                if (relevance > 0.5 && movieIdToTConst.TryGetValue(movieId, out string tconst) && moviesByTConst.TryGetValue(tconst, out Movie movie) && tagIdToTag.TryGetValue(tagId, out string tag))
                {
                    movie.Tags.Add(tag);
                }
            }
        }
    }
}
