using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
	Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);
	Task<IPagedList<Category>> GetCategoriesFilterNameAsync(CategoriesFilterName categoriesFilterName);
}
