namespace ModelLayer.BussinessObject;

public class Artwork
{
    public Artwork()
    {
        ArtworkCategories = new HashSet<ArtworkCategory>();
        ArtworkTags = new HashSet<ArtworkTag>();
        Comments = new HashSet<Comment>();
        LikesNavigation = new HashSet<Like>();
        OrderDetails = new HashSet<OrderDetail>();
    }

    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? PremiumUrl { get; set; }
    public int? Likes { get; set; }
    public decimal? Fee { get; set; }
    public string? Status { get; set; }

    public virtual Account? Account { get; set; }
    public virtual ICollection<ArtworkCategory> ArtworkCategories { get; set; }
    public virtual ICollection<ArtworkTag> ArtworkTags { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> LikesNavigation { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}