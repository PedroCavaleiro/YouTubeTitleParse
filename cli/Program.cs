using YouTubeTitleParse;

Console.Write("Enter a title to split: ");
var title = Console.ReadLine();

if (string.IsNullOrEmpty(title))
    Console.WriteLine("Enter a title to split");
else {
    var data = title.GetArtistTitle();
    Console.WriteLine($"Song \"{data.Title}\" by {data.Artist ?? "Unknown"}");
} 