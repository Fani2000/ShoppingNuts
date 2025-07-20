using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Category { get; set; }

    public string Description { get; set; }

    public string Size { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public string Image { get; set; }
}