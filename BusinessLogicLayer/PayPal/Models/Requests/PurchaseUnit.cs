namespace PayPal.Models.Requests;

public class PurchaseUnit
{
    public string reference_id { get; set; }
    public string description { get; set; }
    public Amount amount { get; set; }
    public List<Item> items { get; set; }
}