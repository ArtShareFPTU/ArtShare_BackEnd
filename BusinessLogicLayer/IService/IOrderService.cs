using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.IService;

public interface IOrderService
{
    Task<List<Order>> GetAllOrderAsync();
    Task<Order> GetOrderByIdAsync(Guid id);
    Task AddOrderAsync(Order Order);
    Task UpdateOrderAsync(Order Order);
    Task DeleteOrderAsync(Guid id);
}