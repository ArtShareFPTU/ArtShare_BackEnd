using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Response.Comment;

namespace ModelLayer.DTOS.Response;

public class ArtworkRespone
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public String Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public int? Likes { get; set; }
    public decimal? Fee { get; set; }
    public List<CommentResponse> Comments { get; set; }
    public List<String> Categorys { get; set; }
    public List<String> Tags { get; set; }

}