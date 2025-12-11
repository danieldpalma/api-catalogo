using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	public CategoryRepository(AppDbContext context) : base(context) { }

	public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters)
	{
		var categories = await GetAllAsync();
		var orderedCategories = categories.OrderBy(c => c.CategoryId).AsQueryable();
		//var result = PagedList<Category>.ToPagedListAsync(orderedCategories, categoriesParameters.PageNumber, categoriesParameters.PageSize);
		var result = await orderedCategories.ToPagedListAsync(categoriesParameters.PageNumber, categoriesParameters.PageSize);

		return result;
	}

	public async Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName)
	{
		var categories = await GetAllAsync();

		if (!string.IsNullOrEmpty(categoriesFilterName.Name))
		{
			categories = categories.Where(c => c.Name!.Contains(categoriesFilterName.Name));
		}

		//var categoriesFiltered = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
		var categoriesFiltered = await categories.ToPagedListAsync(categoriesFilterName.PageNumber, categoriesFilterName.PageSize);

		return categoriesFiltered;
	}
}
