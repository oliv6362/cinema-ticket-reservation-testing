namespace Cinema.Core.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AgeRating { get; set; }
}