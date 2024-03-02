namespace ModelLayer.DTOS.Response;

public class ArtworkRespone
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public int? Likes { get; set; }
    public decimal? Fee { get; set; }
    public List<string> Categorys { get; set; }
    public List<string> Tags { get; set; }
}