namespace Models;
/// <summary> Product </summary>
public class DemoProduct
{
    /// <summary> Id </summary>
    public Guid Id { get; set; }
    /// <summary> Name </summary>
    public string Name { get; set; } = null!;
    /// <summary> Active </summary>
    public bool Active { get; set; }
    /// <summary> Price </summary>
    public double? Price { get; set; }
    /// <summary> Release Date </summary>
    public DateTime? ReleaseDate { get; set; }
    /// <summary> Rating </summary>
    public int Rating { get; set; }
    /// <summary> Product Type </summary>
    public DemoProductType Type { get; set; }
    /// <summary> Extra information in separate table (rarely used) </summary>
    public DemoProductExt? Ext { get; set; }
    /// <summary> Stocks information of Product  </summary>
    public List<DemoInventoryStock> Stocks { get; set; } = new List<DemoInventoryStock>();
}