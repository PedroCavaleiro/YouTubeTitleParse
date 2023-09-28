using System.Text.RegularExpressions;
// ReSharper disable SuggestBaseTypeForParameter

namespace YouTubeTitleParse.Plugins; 

public static class QuotedTitle {
    
    private static readonly List<(string, string)> Quotes = new() { ("“", "”"), ("\x9c", "\x80\xe2\x9d"), ("\"", "\""), ("'", "'") };
    
    private static Regex LooseRegs((string, string) set) {
        var openSet  = set.Item1;
        var closeSet = set.Item2;
        return new Regex(openSet + "(.*?)" + closeSet, RegexOptions.Compiled);
    }
    
    private static Regex StartRegs((string, string) set) {
        var openSet  = set.Item1;
        var closeSet = set.Item2;
        return new Regex("^" + openSet + "(.*?)" + closeSet + "\\s*", RegexOptions.Compiled);
    }

    public static string[] SplitText(string text) {
        foreach (var looseRegx in Quotes.Select(LooseRegs)) {
            text = looseRegx.Replace(text, match => " " + match.Groups[0].Value + " ", 1);
            var match = looseRegx.Match(text);
            if (!match.Success) continue;
            var split  = match.Index;
            var title  = text[split..];
            var artist = text[..split];
            return new[] {artist, title};
        }
        return null;
    }
    
    public static string Clean(string artistOrTitle) =>
        Quotes.Select(StartRegs).Aggregate(artistOrTitle, (text, rx) => rx.Replace(text, "$1 ")).Trim();
    
}