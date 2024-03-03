using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.Enum;

namespace BusinessLogicLayer.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _OrderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

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

    public async Task<Order> AddOrderAsync(List<Carts> cartsList, Guid customerId)
    {
        var order = new Order();
        order.CreateDate = DateTime.Now;
        order.AccountId = customerId;
        order.PaymentMethod = "Paypal";
        order.Id = Guid.NewGuid();
        order.Status = OderStatus.Unpaid.ToString();
        foreach (var item in cartsList)
        {
            var orderDetails = new OrderDetail();
            orderDetails.Id = new Guid();
            orderDetails.OrderId = order.Id;
            orderDetails.Price = item.Price;
            orderDetails.ArtworkId = item.Id;
            order.OrderDetails.Add(orderDetails);
        }
        await _OrderRepository.AddOrderAsync(order);
        return order;
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