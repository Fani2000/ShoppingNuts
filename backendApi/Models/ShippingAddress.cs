using System.ComponentModel.DataAnnotations;

public class ShippingAddress
{
    [Required] public string FullName { get; set; }
    [Required] public string StreetAddress { get; set; }
    [Required] public string City { get; set; }
    [Required] public string State { get; set; }
    [Required] public string ZipCode { get; set; }
    public string Phone { get; set; }
}