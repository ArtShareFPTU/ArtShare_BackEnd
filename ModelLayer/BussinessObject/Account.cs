namespace ModelLayer.BussinessObject;

public class Account
{
    public Account()
    {
        Artworks = new HashSet<Artwork>();
        Comments = new HashSet<Comment>();
        FollowArtists = new HashSet<Follow>();
        FollowFollowers = new HashSet<Follow>();
        InboxReceivers = new HashSet<Inbox>();
        InboxSenders = new HashSet<Inbox>();
        Likes = new HashSet<Like>();
        Orders = new HashSet<Order>();
    }

    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public string? FullName { get; set; }
    public DateTime? Birthday { get; set; }
    public string? UserName { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? Status { get; set; }

    public virtual ICollection<Artwork> Artworks { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Follow> FollowArtists { get; set; }
    public virtual ICollection<Follow> FollowFollowers { get; set; }
    public virtual ICollection<Inbox> InboxReceivers { get; set; }
    public virtual ICollection<Inbox> InboxSenders { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}