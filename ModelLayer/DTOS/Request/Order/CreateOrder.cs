using ModelLayer.BussinessObject;

namespace ModelLayer.DTOS.Request.Order;

public class CreateOrder
{
    public Guid? AccountId { get; set; }
    public decimal? TotalFee { get; set; }
    public DateTime? PaymentDate { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}