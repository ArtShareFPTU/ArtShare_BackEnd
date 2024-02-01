namespace ModelLayer.BussinessObject;

public class Follow
{
    public Guid Id { get; set; }
    public Guid? FollowerId { get; set; }
    public Guid? ArtistId { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual Account? Artist { get; set; }
    public virtual Account? Follower { get; set; }
}