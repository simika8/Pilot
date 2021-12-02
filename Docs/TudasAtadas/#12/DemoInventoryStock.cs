namespace Models;
/// <summary>Stock data</summary>
public class DemoInventoryStock
{
    /// <summary> Id </summary>
    public Guid Id { get; set; }
    /// <summary> Product Id </summary>
    public Guid ProductId { get; set; }
    /// <summary> Store Id </summary>
    public Guid StoreId { get; set; }
    /// <summary> Stock Quantity </summary>
    public double Quantity { get; set; }
}