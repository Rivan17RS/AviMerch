using System.ComponentModel.DataAnnotations;

public class UpdateProductDto
{
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Range(0.01, 999999)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public required string Category { get; set; }

    public string? Province { get; set; }

    public string? ImageUrl { get; set; }
}
