using BusinessLogicLayer.IService;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS;

namespace WebApiLayer.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
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

    /*// GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
      if (_context.Orders == null)
      {
          return NotFound();
      }
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // PUT: api/Order/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(Guid id, Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }*/

    // POST: api/Order
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(List<Carts> cartsList, Guid customerId)
    {
        return await _orderService.AddOrderAsync(cartsList, customerId);
    }

    /*// DELETE: api/Order/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        if (_context.Orders == null)
        {
            return NotFound();
        }
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(Guid id)
    {
        return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
    }*/
}