using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace YouTubeTitleParse.Plugins; 

[SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to \'GeneratedRegexAttribute\'.")]
public static class Common {

    /// <summary>
    /// Cleans common unnecessary info from the title (fluff)
    /// </summary>
    /// <param name="title">Text to clean</param>
    /// <returns>Cleaned title</returns>>
    public static string CleanCommonFluff(string title) {
        // Sub Pop includes "(not the video)" on audio tracks.
        // The " video" part might be stripped by other plugins.
        var cleanedTitle = Regex.Replace(title, @"\(not the( video)?\)\s*$", "");
        // Lyrics videos
        cleanedTitle = Regex.Replace(cleanedTitle, @"(\s*[-~_/]\s*)?\b(with\s+)?lyrics\s*", "", RegexOptions.IgnoreCase);
        cleanedTitle = Regex.Replace(cleanedTitle, @"\(\s*(with\s+)?lyrics\s*\)\s*", "", RegexOptions.IgnoreCase);
        cleanedTitle = Regex.Replace(cleanedTitle, @"\s*\(\s*\)", ""); // ()
        return cleanedTitle;
    }
    
}