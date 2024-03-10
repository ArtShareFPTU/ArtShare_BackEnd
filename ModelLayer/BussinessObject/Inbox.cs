namespace ModelLayer.BussinessObject;

public class Inbox
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }

    public virtual Account? Receiver { get; set; }
    public virtual Account? Sender { get; set; }
}