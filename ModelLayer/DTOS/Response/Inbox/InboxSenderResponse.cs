namespace ModelLayer.DTOS.Response.Inbox;

public class InboxSenderResponse
{
    public Guid Id { get; set; }
    public string Receiver { get; set; }
    public string? Title { get; set; }
}