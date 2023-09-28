// ReSharper disable UseVerbatimString

using System.Text.RegularExpressions;

namespace YouTubeTitleParse.Plugins; 

public static class RemoveFileExtensions {
    
    private static readonly string[] VideoExtensions = {
        "3g2",
        "3gp",
        "aaf",
        "asf",
        "avchd",
        "avi",
        "drc",
        "flv",
        "m2v",
        "m4p",
        "m4v",
        "mkv",
        "mng",
        "mov",
        "mp2",
        "mp4",
        "mpe",
        "mpeg",
        "mpg",
        "mpv",
        "mxf",
        "nsv",
        "ogg",
        "ogv",
        "qt",
        "rm",
        "rmvb",
        "roq",
        "svi",
        "vob",
        "webm",
        "wmv",
        "yuv"
    };
    private static readonly string[] AudioExtensions = {
        "wav",
        "bwf",
        "raw",
        "aiff",
        "flac",
        "m4a",
        "pac",
        "tta",
        "wv",
        "ast",
        "aac",
        "mp2",
        "mp3",
        "mp4",
        "amr",
        "s3m",
        "3gp",
        "act",
        "au",
        "dct",
        "dss",
        "gsm",
        "m4p",
        "mmf",
        "mpc",
        "ogg",
        "oga",
        "opus",
        "ra",
        "sln",
        "vox"
    };
    private static readonly string[] FileExtensions = ConcatArrays(VideoExtensions, AudioExtensions);

    private static readonly Regex FileExtensionsRegex =
        new Regex(@"\.(" + "|" + string.Join("|", FileExtensions) + ")$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string CleanFileExtensions(string title) => FileExtensionsRegex.Replace(title, "");

    /// <summary>
    /// Concat twi or more arrays 
    /// </summary>
    /// <param name="list">List of arrays to concat</param>
    /// <typeparam name="T">Type of the arrays</typeparam>
    /// <returns>New array</returns>
    private static T[] ConcatArrays<T>(params T[][] list) {
        var result = new T[list.Sum(a => a.Length)];
        var offset = 0;
        foreach (var t in list) {
            t.CopyTo(result, offset);
            offset += t.Length;
        }
        return result;
    }
    
}