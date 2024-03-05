using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.IRepository;

public interface IOrderRepository
{
    Task<Order> GetOrderByTokenAsync(string token);
    Task<List<Order>> GetAllOrderAsync();
    Task<Order> GetOrderByIdAsync(Guid id);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid id);
}