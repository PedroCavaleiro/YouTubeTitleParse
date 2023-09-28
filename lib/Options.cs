// ReSharper disable UnusedAutoPropertyAccessor.Global

using YouTubeTitleParse.Plugins;

namespace YouTubeTitleParse; 

public class Options {
    public List<Func<string, string>>   Before { get; set; }
    public List<Func<string, string[]>> Split  { get; set; }
    public AfterCleanup                 After  { get; set; }

    public class AfterCleanup {
        public List<Func<string, string>> TitleCleaner  { get; set; }
        public List<Func<string, string>> ArtistCleaner { get; set; }
    }

    public void LoadDefaults() {
        Before = new List<Func<string, string>> { RemoveFileExtensions.CleanFileExtensions, Base.CleanFluff };
        Split  = new List<Func<string, string[]>> { Base.SplitArtistTitle, QuotedTitle.SplitText };
        After = new AfterCleanup {
            TitleCleaner  = new List<Func<string, string>> { Base.CleanTitle, QuotedTitle.Clean, Common.CleanCommonFluff },
            ArtistCleaner = new List<Func<string, string>> { Base.CleanArtist, QuotedTitle.Clean },
        };
    }

    internal void OverrideDefaults(Options options) {
        if (options.Before != null || options.Before?.Count > 0)
            Before = options.Before;
        if (options.Split != null || options.Split?.Count > 0)
            Split = options.Split;
        if (options.After != null)
            After = options.After;
    }
}