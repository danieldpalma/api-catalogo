using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
	Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
	Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterPrice);
	Task<IEnumerable<Product>> GetProductByCategoryAsync(int id);
}
