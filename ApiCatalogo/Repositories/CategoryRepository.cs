using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	public CategoryRepository(AppDbContext context) : base(context) { }

	public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
	{
		var categories = GetAll().OrderBy(c => c.CategoryId).AsQueryable();
		var orderedCategories = PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize);

		return orderedCategories;
	}
}
