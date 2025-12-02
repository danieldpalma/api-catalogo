using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Models;

[Table("Products")]
public class Product
{
	[Key]
	public int ProductId { get; set; }
	[Required(ErrorMessage = "Name is required.")]
	[StringLength(80, ErrorMessage = "The name must be between 5 and 80 characters.", MinimumLength = 5)]
	public string? Name { get; set; }
	[Required]
	[StringLength(80, ErrorMessage = "The description must be between 5 and 80 characters.", MinimumLength = 5)]
	public string? Description { get; set; }
	[Required]
	[Column(TypeName ="decimal(10,2)")]
	[Range(1, 1000000, ErrorMessage = "The price must be between {1} and {2}")]
	public decimal Price { get; set; }
	[Required]
	[StringLength(300)]
	public string? ImageUrl { get; set; }
	public float Supply { get; set; }
	public DateTime CreatedAt { get; set; }
	public int CategoryId { get; set; }
	[JsonIgnore]
	public Category? Category { get; set; }
}
