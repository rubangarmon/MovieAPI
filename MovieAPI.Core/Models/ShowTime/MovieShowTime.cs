namespace MovieAPI.Core.Models.ShowTime;

public class MovieShowTime
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public List<string> Languages { get; set; } = [];
    public int ReleaseYear { get; set; }
}
