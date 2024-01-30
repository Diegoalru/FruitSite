using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FruitWebApp.Models;

public class FruitModel
{
    [Key] [JsonPropertyName("id")] public int Id { get; set; }


    [Display(Name = "Fruit Name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [Display(Name = "Available?")]
    [JsonPropertyName("inStock")]
    public bool InStock { get; set; }
}