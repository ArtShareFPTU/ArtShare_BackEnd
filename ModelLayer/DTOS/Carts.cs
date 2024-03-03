namespace ModelLayer.DTOS;

public class Carts
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}