using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public interface ICategoryRepository
{
	IEnumerable<Category> GetCategories();
	IEnumerable<Category> GetCategoriesWithProducts();
	Category GetCategoryById(int id);
	Category CreateCategory(Category category);
	Category UpdateCategory(int id, Category category);
	Category DeleteCategory(int id);
}
