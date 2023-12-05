# YouTubeTitleParse
Parse song &amp; artist names from YouTube video titles

## Description

It parses the artist and the song title based on YouTube titles. This library does not make any calls to any API, it tries to recognize based on the most common patters.

## Requirements

* For the CLI program
    * [.NET 7 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* To build or use in your project
    * [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## CLI Usage

Download the latest version of YouTubeTitleParser and run the command

```
dotnet YouTubeTitleParse-CLI
```

## Library

1. Add the nuget to your project or download the .dll from the latest release
2. Import the library `using YouTubeTitleParse;`
3. The library works as a `String` extension so you can just call the function `GetArtistTitle()` on your variable like so

```csharp
"ARTIST - Song (Official Video)".GetArtistTitle();
```

**NOTE**: If it's not possible to split into Artist and Title it will default to title only and the artist **will** be null

## Options

It's possible to remove the functions running or even add your own

There are two types of functions for the processor, splitters and cleaners

 * The cleaner function has the following header `string Foo(string videoTitle)`
 * The splitter function has the following header `string[] Foo(string videoTitle)`

 For the splitter the first item of the array is the artist and the second item is the song title

How to use Options

In this Example we will add a custom cleaner, and remove the FileExtension cleaner and keep the same splitters
```csharp
using YouTubeTitleParse.Plguins;
using YouTubeTitleParse;

public string MyCleanerFunc(string title) {
    // [...]
}


var options = new Options() {
    Before = new List<Func<string, string>> { Base.CleanFluff, MyCleanerFunc },
    After = new Options.AfterCleanup {
        TitleCleaner  = new List<Func<string, string>> { Base.CleanTitle, QuotedTitle.Clean, Common.CleanCommonFluff },
        ArtistCleaner = new List<Func<string, string>> { Base.CleanArtist, QuotedTitle.Clean, MyCleanerFunc }
    }
};

"ARTIST - Song (Official Video)".GetArtistTitle(options);
```

## Credits

This is a rewrite for C# of the Python3 [youtube-title-parse](https://github.com/lttkgp/youtube_title_parse)

## Contributing

Feel free to create pull requests, open issues specially if you have a new regex to parse more titles.

Note that each Regex has the comment identifying what it does to avoid having multiple regex doing the same.

# License

YouTubeTitleParser is made available under [MIT License](LICENSE)