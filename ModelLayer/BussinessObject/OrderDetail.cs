﻿namespace ModelLayer.BussinessObject;

public class OrderDetail
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? ArtworkId { get; set; }
    public decimal Price { get; set; }
    

    public virtual Artwork? Artwork { get; set; }
    public virtual Order? Order { get; set; }
}