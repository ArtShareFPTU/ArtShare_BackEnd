using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IOrderDetailRepository
{
    Task<List<OrderDetail>> GetAllOrderDetailAsync();
    Task<OrderDetail> GetOrderDetailByIdAsync(Guid id);
    Task AddOrderDetailAsync(OrderDetail orderDetail);
    Task UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task DeleteOrderDetailAsync(Guid id);
    Task<List<OrderDetail>> GetOrderDetailByAccountId(Guid accountId);
    Task<List<Artwork>> GetArtworksByOrderId(Guid orderId);
}