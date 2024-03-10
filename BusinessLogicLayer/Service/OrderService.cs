using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Request.Order;
using ModelLayer.Enum;

namespace BusinessLogicLayer.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _OrderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public OrderService(IOrderRepository OrderRepository, IOrderDetailRepository orderDetailRepository)
    {
        _OrderRepository = OrderRepository;
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<List<Order>> GetAllOrderAsync()
    {
        return await _OrderRepository.GetAllOrderAsync();
    }

    public async Task<List<Order>> GetOrderByAccountId(Guid id)
    {
        return await _OrderRepository.GetOrderByAccountId(id);
    }

    public async Task<Order> AddOrderAsync(List<Carts> cartsList, Guid customerId)
    {
        var order = new Order();
        order.CreateDate = DateTime.Now;
        order.AccountId = customerId;
        order.PaymentMethod = "Paypal";
        order.Id = Guid.NewGuid();
        order.Status = OrderStatus.Processing.ToString();
        foreach (var item in cartsList)
        {
            var orderDetails = new OrderDetail();
            orderDetails.Id = Guid.NewGuid();
            orderDetails.OrderId = order.Id;
            orderDetails.Price = item.Price;
            orderDetails.ArtworkId = item.Id;
            order.OrderDetails.Add(orderDetails);
        }
        await _OrderRepository.AddOrderAsync(order);
        return order;
    }
    public async Task CreateTokenAsync(CreateToken token)
    {
        var order = await _OrderRepository.GetOrderByIdAsync(token.Id);
        order.Token = token.Token;
        await _OrderRepository.UpdateOrderAsync(order);
    }

    public async Task UpdateToken(string token, string result)
    {
        var order = await _OrderRepository.GetOrderByTokenAsync(token);
        if (result.Equals("Success"))
        {
            order.Status = OrderStatus.Completed.ToString();            
        }
        else
        {
            order.Status = OrderStatus.Cancelled.ToString();           
        }
        await _OrderRepository.UpdateOrderAsync(order); 
    }


    public async Task UpdateOrderAsync(Order Order)
    {
        await _OrderRepository.UpdateOrderAsync(Order);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        await _OrderRepository.DeleteOrderAsync(id);
    }

    public async Task<List<OrderDetail>> GetOrderDetailByAccountId(Guid id)
    {
        var ord = await _orderDetailRepository.GetOrderDetailByAccountId(id);
        return ord;
    }
}