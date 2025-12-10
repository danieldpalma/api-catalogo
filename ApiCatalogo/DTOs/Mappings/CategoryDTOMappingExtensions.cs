using ApiCatalogo.Models;

namespace ApiCatalogo.DTOs.Mappings;

public static class CategoryDTOMappingExtensions
{
	public static CategoryDTO? ToCategoryDTO(this Category category)
	{
		if (category is null)
			return null;

		var categoryDTO = new CategoryDTO()
		{
			CategoryId = category.CategoryId,
			Name = category.Name,
			ImageUrl = category.ImageUrl,
		};

		return categoryDTO;
	}

	public static Category? ToCategory(this CategoryDTO categoryDTO)
	{
		if (categoryDTO is null)
			return null;

		var category = new Category()
		{
			CategoryId = categoryDTO.CategoryId,
			Name = categoryDTO.Name,
			ImageUrl = categoryDTO.ImageUrl,
		};

		return category;
	}

	public static IEnumerable<CategoryDTO> ToCategoryDTOList(this IEnumerable<Category> categories)
	{
		if (categories is null || !categories.Any())
		{
			return [];
		}

		return categories.Select(category => new CategoryDTO
		{
			CategoryId = category.CategoryId,
			Name = category.Name,
			ImageUrl = category.ImageUrl,
		}).ToList();
	}
}
