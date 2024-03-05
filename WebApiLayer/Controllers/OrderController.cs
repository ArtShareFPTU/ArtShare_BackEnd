using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;
using ModelLayer.DTOS.Request.Order;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/Order
    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetOrders()
    {
        return await _orderService.GetAllOrderAsync();
    }

    // // GET: api/Order/5
    // [HttpGet("{id}")]
    // public async Task<ActionResult<Order>> GetOrder(Guid id)
    // {
    //   if (_context.Orders == null)
    //   {
    //       return NotFound();
    //   }
    //     var order = await _context.Orders.FindAsync(id);
    //
    //     if (order == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return order;
    // }

    // PUT: api/Order/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> CreateTokenOrder(CreateToken token)
    {
        await _orderService.CreateTokenAsync(token);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateStatusOrder(UpdateToken token)
    {
        await _orderService.UpdateToken(token.token, token.result);
        return NoContent();
    }

    // POST: api/Order
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(List<Carts> cartsList, Guid customerId)
    {
        return await _orderService.AddOrderAsync(cartsList, customerId);
    }
    
}