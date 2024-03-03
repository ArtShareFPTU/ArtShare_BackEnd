namespace ModelLayer.BussinessObject;

public class Comment
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? ArtworkId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
    public virtual Account? Account { get; set; }
    public virtual Artwork? Artwork { get; set; }
}