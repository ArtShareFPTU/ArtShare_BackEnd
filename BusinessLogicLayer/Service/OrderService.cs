using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;

namespace BusinessLogicLayer.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _OrderRepository;

    public OrderService(IOrderRepository OrderRepository)
    {
        _OrderRepository = OrderRepository;
    }

    public async Task<List<Order>> GetAllOrderAsync()
    {
        return await _OrderRepository.GetAllOrderAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _OrderRepository.GetOrderByIdAsync(id);
    }

    public async Task AddOrderAsync(Order Order)
    {
        await _OrderRepository.AddOrderAsync(Order);
    }

    public async Task UpdateOrderAsync(Order Order)
    {
        await _OrderRepository.UpdateOrderAsync(Order);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        await _OrderRepository.DeleteOrderAsync(id);
    }
}