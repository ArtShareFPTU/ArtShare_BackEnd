using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer.BussinessObject;

namespace DataAccessLayer.BussinessObject.Repository;

public class OderDetailRepository : IOrderDetailRepository
{
    private readonly ArtShareContext _context;

    public OderDetailRepository()
    {
        _context = new ArtShareContext();
    }

    public async Task<List<OrderDetail>> GetAllOrderDetailAsync()
    {
        return await _context.OrderDetails.Include(c => c.Artwork).Include(c => c.Order).ThenInclude(c => c.Account).ToListAsync();
    }

    public async Task<OrderDetail> GetOrderDetailByIdAsync(Guid id)
    {
        return await _context.OrderDetails.FindAsync(id);
    }

    public async Task AddOrderDetailAsync(OrderDetail orderDetail)
    {
        _context.OrderDetails.Add(orderDetail);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
        _context.Entry(orderDetail).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrderDetailAsync(Guid id)
    {
        var orderDetails = await _context.OrderDetails.FindAsync(id);
        _context.OrderDetails.Remove(orderDetails);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderDetail>> GetOrderDetailByAccountId(Guid accountId)
    {
        var orderdetail = await _context.OrderDetails.Include(c => c.Order).ThenInclude(a => a.Account).Include(c => c.Artwork).Where(c => c.Artwork.AccountId == accountId).ToListAsync();
        return orderdetail;
    }
}