using System.ComponentModel.DataAnnotations;
namespace Models;

/// <summary> Extra information in separate table (rarely used) </summary>
public class DemoProductExt
{
    /// <summary> Id. Equals to Product.Id </summary>
    [Key]
    public Guid ProductId { get; set; }
    /// <summary> Description </summary>
    public string? Description { get; set; }
    /// <summary> Minimum Stock </summary>
    public double? MinimumStock { get; set; }
}

