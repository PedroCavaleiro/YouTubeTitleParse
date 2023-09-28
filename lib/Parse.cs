namespace YouTubeTitleParse; 

public static class Parse {

    /// <summary>
    /// Gets the artist and the title
    /// </summary>
    /// <param name="text">The title to process</param>
    /// <param name="options">Options defining which plugins to run and the order. When null the default will be loaded</param>
    /// <returns>Object with the Artist (can be null) and Title</returns>
    public static ArtistTitle GetArtistTitle(this string text, Options options = null) {
        var loadedOptions = new Options();
        loadedOptions.LoadDefaults();
        if (options != null)
            loadedOptions.OverrideDefaults(options);

        return Core.GetSongArtistTitle(text, loadedOptions);
    }
    
}