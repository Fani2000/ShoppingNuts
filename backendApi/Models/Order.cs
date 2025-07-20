using System.ComponentModel.DataAnnotations;
using backend.Models;

public class Order
{
    public string Id { get; set; }

    [Required] public DateTime Date { get; set; }

    [Required] public decimal Total { get; set; }
    
    public string? CustomerName { get; set; }
    public string? Status{ get; set; }
    
    [Required] public DateTime OrderDate { get; set; }
    [Required] public List<CartItem> Items { get; set; } = new List<CartItem>();
    
    // Add shipping address
    public ShippingAddress ShippingAddress { get; set; }
}