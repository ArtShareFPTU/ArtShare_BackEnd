namespace ModelLayer.PayPal;

public class purchase_units
{
    public string reference_id { get; set; }
    public amount amount { get; set; }
    public List<items> items { get; set; }
}