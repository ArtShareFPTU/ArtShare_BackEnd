namespace ModelLayer.BussinessObject;

public class ArtworkCategory
{
    public Guid Id { get; set; }
    public Guid? ArtworkId { get; set; }
    public Guid? CategoryId { get; set; }

    public virtual Artwork? Artwork { get; set; }
    public virtual Category? Category { get; set; }
}