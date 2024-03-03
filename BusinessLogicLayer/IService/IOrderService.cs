using ModelLayer.BussinessObject;
using ModelLayer.DTOS;

namespace BusinessLogicLayer.IService;

public interface IOrderService
{
    Task<List<Order>> GetAllOrderAsync();
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<Order> AddOrderAsync(List<Carts> cartsList, Guid customerId);
    Task UpdateOrderAsync(Order Order);
    Task DeleteOrderAsync(Guid id);
}