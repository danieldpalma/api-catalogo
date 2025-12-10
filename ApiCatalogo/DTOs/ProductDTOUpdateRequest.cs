using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
	[Range(1, 9999, ErrorMessage = "Stock must be between 1 and 9999")]
	public float Supply { get; set; }

	public DateTime CreatedAt { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (CreatedAt.Date <= DateTime.Now.Date)
		{
			yield return new ValidationResult("Date must be later then today", new[] { nameof(this.CreatedAt) });
		}
	}
}
