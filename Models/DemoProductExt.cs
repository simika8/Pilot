using System.ComponentModel.DataAnnotations;
namespace Models;

public class DemoProductExt
{
    [Key]
    public Guid ProductId { get; set; }
    public string? Description { get; set; }
    public double? MinimumStock { get; set; }
}

