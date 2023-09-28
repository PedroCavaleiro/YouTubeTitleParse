namespace YouTubeTitleParse; 

public static class Core {

    /// <summary>
    /// Performs the clean ups
    /// </summary>
    /// <param name="title">Text to cleanup</param>
    /// <param name="functions">Functions to execute</param>
    /// <returns>Cleaned up title</returns>
    private static string ExecBefore(this string title, IEnumerable<Func<string, string>> functions) =>
        functions.Aggregate(title, (current, func) => func(current));
    
    /// <summary>
    /// Tries to split the title
    /// It will run until the a splitter produces a artist and title
    /// If it's not possible to split the the array will provide null for artist and the title for the song title
    /// </summary>
    /// <param name="title">Title to split</param>
    /// <param name="splitters">Splitter functions to execute</param>
    /// <returns></returns>
    private static string[] ExecSplitter(this string title, IEnumerable<Func<string, string[]>> splitters) {
        var result = Array.Empty<string>();
        foreach (var splitter in splitters) {
            var split = splitter(title);
            if (split == null) {
                result = new[] { null, title };
                continue;
            }
            
            result = split.Length switch {
                1 => new[] { null, split[0] },
                2 => new[] { split[0], split[1] },
                _ => new[] { null, title }
            };
            if (split.Length == 2)
                return result;
        }

        return result;
    }

    /// <summary>
    /// Performs the last cleanup
    /// </summary>
    /// <param name="data">Data to cleanup</param>
    /// <param name="functions">Functions defined on Options.AfterCleanup to clean the artist and title</param>
    /// <returns></returns>
    private static ArtistTitle ExecAfter(this IReadOnlyList<string> data, Options.AfterCleanup functions) {
        var title  = data[1];
        var artist = data[0];
        if (data[0] != null)
            artist = functions.ArtistCleaner.Aggregate(artist, (current, func) => func(current));
        title = functions.TitleCleaner.Aggregate(title, (current, func) => func(current));

        return new ArtistTitle {
            Title  = title,
            Artist = artist
        };
    }

    public static ArtistTitle GetSongArtistTitle(string text, Options options) =>
        text.ExecBefore(options.Before).ExecSplitter(options.Split).ExecAfter(options.After);

}