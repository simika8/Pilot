namespace Models;
/// <summary> Product </summary>
public class DemoProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Active { get; set; }
    public double? Price { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int Rating { get; set; }
    public DemoProductType Type { get; set; }
    public DemoProductExt? Ext { get; set; }
    public List<DemoInventoryStock> Stocks { get; set; } = new List<DemoInventoryStock>();
}