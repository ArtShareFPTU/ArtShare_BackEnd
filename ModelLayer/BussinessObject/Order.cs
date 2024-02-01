namespace ModelLayer.BussinessObject;

public class Order
{
    public Order()
    {
        OrderDetails = new HashSet<OrderDetail>();
    }

    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public decimal? TotalFee { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? Status { get; set; }

    public virtual Account? Account { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}