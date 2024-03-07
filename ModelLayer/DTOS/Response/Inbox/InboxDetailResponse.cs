namespace ModelLayer.DTOS.Response.Inbox;

public class InboxDetailResponse
{
    public Guid Id { get; set; }
    public String Receiver { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
}