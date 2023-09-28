using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace YouTubeTitleParse.Plugins;

[SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to \'GeneratedRegexAttribute\'.")]
public static class Base {

    /// <summary>
    /// Cleans the song title
    /// </summary>
    /// <param name="text">The string to clean</param>
    /// <returns>Cleaned song title</returns>
    public static string CleanTitle(string text) {
        var title = text.Trim();
        title = CleanFluff(title);
        title = Regex.Replace(title, @"\s*\*+\s?\S+\s?\*+$",         "");                          // **NEW**
        title = Regex.Replace(title, @"\s*video\s*clip",             "", RegexOptions.IgnoreCase); // video clip
        title = Regex.Replace(title, @"\s+\(?live\)?$",              "", RegexOptions.IgnoreCase); // live
        title = Regex.Replace(title, @"\(\s*\)",                     "");                          // Leftovers after e.g. (official video)
        title = Regex.Replace(title, @"\[\s*]",                      "");                          // Leftovers after e.g [1080p]
        title = Regex.Replace(title, @"【\s*】",                       "");                         // Leftovers after e.g【MV
        title = Regex.Replace(title, """^(|.*\s)\"(.*)\"(\s.*|)$""", @"\2");                       // Artist - The new "Track title" featuring someone
        title = Regex.Replace(title, @"^(|.*\s)'(.*)'(\s.*|)$",      @"\2");                       // 'Track title'
        title = Regex.Replace(title, @"^[/\s,:;~\-–_\s\""]+",        "");                          // trim starting white chars and dash
        title = Regex.Replace(title, @"[/\s,:;~\-–_\s\""]+$",        "");                          // trim trailing white chars and dash
        return title;
    }
    
    /// <summary>
    /// Cleans the artist name
    /// </summary>
    /// <param name="text">The string to clean</param>
    /// <returns>Cleaned artist name</returns>
    public static string CleanArtist(string text) {
        var artist = text.Trim();
        artist = CleanFluff(artist);
        artist = Regex.Replace(artist, @"\s*[0-1][0-9][0-1][0-9][0-3][0-9]\s*", ""); // date formats ex. 130624
        artist = Regex.Replace(artist, @"^[/\s,:;~\-–_\s\""]+",                 ""); // trim starting white chars and dash
        artist = Regex.Replace(artist, @"[/\s,:;~\-–_\s\""]+$",                 ""); // trim trailing white chars and dash
        return artist;
    }
        
    /// <summary>
    /// Cleans various versions of "MV" and "PV" markers
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string CleanMvPv(string text) {
        var cleanedText = Regex.Replace(text, @"\s*\[\s*(?:off?icial\s+)?([PM]\/?V)\s*]", "", RegexOptions.IgnoreCase); // [MV] or [M/V]
        cleanedText = Regex.Replace(cleanedText, @"\s*\(\s*(?:off?icial\s+)?([PM]\/?V)\s*\)",  "", RegexOptions.IgnoreCase); // (MV) or (M/V)
        cleanedText = Regex.Replace(cleanedText, @"\s*【\s*(?:off?icial\s+)?([PM]\/?V)\s*】",   "", RegexOptions.IgnoreCase); //【MV】 or 【M/V】
        cleanedText = Regex.Replace(cleanedText, @"[\s\-–_]+(?:off?icial\s+)?([PM]\/?V)\s*", "", RegexOptions.IgnoreCase);   // MV or M/V at the end
        cleanedText = Regex.Replace(cleanedText, @"(?:off?icial\s+)?([PM]\/?V)[\s\-–_]+", "", RegexOptions.IgnoreCase);      // MV or M/V at the start
        return cleanedText;
    }
    
    /// <summary>
    /// Removes unnecessary info (fluff)
    /// All REGEX ignores case
    /// </summary>
    /// <param name="text">The string to clean</param>
    /// <returns>Cleaned string</returns>
    public static string CleanFluff(string text) {
        var cleanedText = CleanMvPv(text);
        cleanedText = Regex.Replace(cleanedText, @"\s*\[[^\]]+]$",                        "", RegexOptions.IgnoreCase); // [whatever] at the end
        cleanedText = Regex.Replace(cleanedText, @"^\s*\[[^\]]+]\s*",                     "", RegexOptions.IgnoreCase); // [whatever] at the start
        cleanedText = Regex.Replace(cleanedText, @"\s*\([^)]*\bver(\.|sion)?\s*\)$",      "", RegexOptions.IgnoreCase); // (whatever version)
        cleanedText = Regex.Replace(cleanedText, @"\s*[a-z]*\s*\bver(\.|sion)?$",         "", RegexOptions.IgnoreCase); // ver. & 1 word before (no parens)
        cleanedText = Regex.Replace(cleanedText, @"\s*(of+icial\s*)?(music\s*)?video",    "", RegexOptions.IgnoreCase); // (official)? (music)? video
        cleanedText = Regex.Replace(cleanedText, @"\s*(full\s*)?album",                   "", RegexOptions.IgnoreCase); // (full)? album
        cleanedText = Regex.Replace(cleanedText, @"\s*(ALBUM TRACK\s*)?(album track\s*)", "", RegexOptions.IgnoreCase); // (ALBUM TRACK)
        cleanedText = Regex.Replace(cleanedText, @"\s*\(\s*of+icial\s*\)",                "", RegexOptions.IgnoreCase); // (official)
        cleanedText = Regex.Replace(cleanedText, @"\s*\(\s*lyric(s)?\s*\)",               "", RegexOptions.IgnoreCase); // (lyrics)
        cleanedText = Regex.Replace(cleanedText, @"\s*\(\s*(of+icial)?\s*lyric(s)?\s*\)", "", RegexOptions.IgnoreCase); // (official lyrics)
        cleanedText = Regex.Replace(cleanedText, @"\s*\(\s*[0-9]{4}\s*\)",                "", RegexOptions.IgnoreCase); // (1999)
        cleanedText = Regex.Replace(cleanedText, @"\s+\(\s*(HD|HQ|[0-9]{3,4}p|4K)\s*\)$", "", RegexOptions.IgnoreCase); // (HD) (HQ) (1080p) (4K)
        cleanedText = Regex.Replace(cleanedText, @"[\s\-–_]+(HD|HQ|[0-9]{3,4}p|4K)\s*$",  "", RegexOptions.IgnoreCase); // - HD - HQ - 720p - 4K
        return cleanedText;
    }
    
    /// <summary>
    /// Checks if the text is in quotes
    /// We consider quotes the characters defined in openChars/closeChars
    /// </summary>
    /// <param name="text">Text to verify</param>
    /// <param name="endIndex">End index to verify</param>
    /// <returns></returns>
    private static bool InQuotes(string text, int endIndex) {
        var openChars   = new List<char> { '(', '[', '{', '«' };
        var closeChars  = new List<char> { ')', ']', '}', '»' };
        var toggleChars = new List<char> { '"', '\'' };
        var openPars = new Dictionary<char, int> { 
            { ')', 0 }, { ']', 0 }, { '}', 0 }, { '»', 0 }, { '"', 0 }, { '\'', 0 } 
        };
        foreach (var character in text[..endIndex]) {
            var index = openChars.IndexOf(character);
            if (index != -1)
                openPars[closeChars[index]] += 1;
            else if (closeChars.IndexOf(character) != -1 && openPars[character] > 0)
                openPars[character] -= 1;
            if (toggleChars.IndexOf( character) != -1)
                openPars[character] = 1 - openPars[character];
        }

        return openPars.Values.Aggregate((acc, value) => acc + value) > 0;
    }

    /// <summary>
    /// Splits the text at the separators
    /// </summary>
    /// <param name="text">Text to split</param>
    /// <returns>Tuple containing the artist and title</returns>
    public static string[] SplitArtistTitle(string text) {
        return (from separator in Separators let idx = text
                    .IndexOf(separator, StringComparison.InvariantCulture) 
                        where idx > -1 && !InQuotes(text, idx) 
                        select new[] { text[..idx], ClearBrandingFromTitle(text[(idx + separator.Length)..]) })
            .FirstOrDefault();
    }

    /// <summary>
    /// Clears the label branding from the song title
    /// Branding usually presents with (song title | LABEL)
    /// </summary>
    /// <param name="title">Song Title</param>
    /// <returns>Unbranded and trimmed song title</returns>
    private static string ClearBrandingFromTitle(string title) {
        return title.Split("|")[0].Trim();
    }

    private static readonly string[] Separators = {
        " -- ",
        "--",
        " - ",
        " – ",
        " — ",
        " _ ",
        "-",
        "–",
        "—",
        ":",
        "|",
        "///",
        " / ",
        "_",
        "/"
    };
    
}