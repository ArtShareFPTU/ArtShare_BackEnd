namespace ModelLayer.BussinessObject;

public class Tag
{
    public Tag()
    {
        ArtworkTags = new HashSet<ArtworkTag>();
    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ArtworkTag> ArtworkTags { get; set; }
}