using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
	PagedList<Category> GetCategories(CategoriesParameters categoriesParameters);
	PagedList<Category> GetCategoriesFilterName(CategoriesFilterName categoriesFilterName);
}
