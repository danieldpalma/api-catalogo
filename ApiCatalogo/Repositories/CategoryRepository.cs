using ApiCatalogo.Context;
using ApiCatalogo.Extensions;
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

	public PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesFilterName)
	{
		var categories = GetAll().AsQueryable();

		if (!string.IsNullOrEmpty(categoriesFilterName.Name))
		{
			categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
		}

		var categoriesFiltered = PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
		return categoriesFiltered;
	}
}
