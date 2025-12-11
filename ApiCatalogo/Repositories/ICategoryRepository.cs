using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
	Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);
	Task<PagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName);
}
