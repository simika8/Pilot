namespace Models;
public class DemoInventoryStock
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public double Quantity { get; set; }
}