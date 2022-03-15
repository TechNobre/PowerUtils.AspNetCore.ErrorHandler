using System.ComponentModel.DataAnnotations;

namespace PowerUtils.AspNetCore.ErrorHandler.Samples.Models;

public class Product
{
    [Required]
    public string Name { get; set; }

    [MaxLength(2)]
    public string Description { get; set; }

    public int Value { get; set; }

    public ProductDetails Details { get; set; }
}

public class ProductDetails
{
    public string Color { get; set; }
    public double Height { get; set; }
}
