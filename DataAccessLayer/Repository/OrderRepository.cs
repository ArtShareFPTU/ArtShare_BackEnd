using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ArtShareContext _context;

    public OrderRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<Order> GetOrderByTokenAsync(string token)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.Token.Equals(token));
    }

    public async Task<List<Order>> GetAllOrderAsync()
    {
        return await _context.Orders.Include(c => c.Account).Include(c => c.OrderDetails).ThenInclude(a => a.Artwork).ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var old = await GetOrderByIdAsync(order.Id);
        old.Token = order.Token;
        _context.Orders.Update(old);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrderByAccountId(Guid accountId)
    {
        return await _context.Orders.Include(c => c.Account).Where(c => c.AccountId == accountId).ToListAsync();
    }
}