using System.ComponentModel.DataAnnotations;

namespace FruitAPI;

public class Fruit
{
    public int Id { get; set; }

    [Required] [StringLength(16)] public required string? Name { get; set; }

    public bool InStock { get; set; }
}