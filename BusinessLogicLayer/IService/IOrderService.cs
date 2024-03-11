using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Request.Order;

namespace BusinessLogicLayer.IService;

public interface IOrderService
{
    Task<List<Order>> GetAllOrderAsync();
    Task<List<Order>> GetOrderByAccountId(Guid id);
    Task<Order> AddOrderAsync(List<Carts> cartsList, Guid customerId);
    Task UpdateToken(string token, string result);
    Task CreateTokenAsync(CreateToken updateToken);
    Task UpdateOrderAsync(Order Order);
    Task DeleteOrderAsync(Guid id);
    Task<List<OrderDetail>> GetOrderDetailByAccountId(Guid id);
}