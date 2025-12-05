using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalogo.Models;

[Table("Categories")]
public class Category
{
	[Key]
	public int CategoryId { get; set; }
	[Required]
	[StringLength(80)]
	public string? Name { get; set; }
	[Required]
	[StringLength(300)]
	public string? ImageUrl { get; set; }
	[JsonIgnore]
	public ICollection<Product>? Products { get; set; } = [];
}
