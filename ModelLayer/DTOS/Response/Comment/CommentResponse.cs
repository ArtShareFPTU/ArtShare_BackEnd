namespace ModelLayer.DTOS.Response.Comment;

public class CommentResponse
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
}