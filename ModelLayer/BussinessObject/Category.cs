namespace ModelLayer.BussinessObject;

public class Category
{
    public Category()
    {
        ArtworkCategories = new HashSet<ArtworkCategory>();
    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual ICollection<ArtworkCategory> ArtworkCategories { get; set; }
}