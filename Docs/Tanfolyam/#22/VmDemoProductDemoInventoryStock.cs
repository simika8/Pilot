namespace Models;
/// <summary>Product + Stock data</summary>
public class VmDemoProductDemoInventoryStock
{
    /// <summary> Id </summary>
    public Guid Id { get; set; }

    /// <summary> Product </summary>
    public DemoProduct DemoProduct { get; set; } = null!;
    /// <summary> Stock in a specified Store  </summary>
    public DemoInventoryStock? DemoInventoryStock { get; set; }
}