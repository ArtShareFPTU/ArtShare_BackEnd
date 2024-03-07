namespace ModelLayer.DTOS.Response.Inbox;

public class InboxReceiverResponse
{
    public Guid Id { get; set; }
    public string Sender { get; set; }
    public string? Title { get; set; }
}