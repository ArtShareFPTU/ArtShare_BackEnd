namespace ModelLayer.BussinessObject;

public class ArtworkTag
{
    public Guid Id { get; set; }
    public Guid? ArtworkId { get; set; }
    public Guid? TagId { get; set; }

    public virtual Artwork? Artwork { get; set; }
    public virtual Tag? Tag { get; set; }
}