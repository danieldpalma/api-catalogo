using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
	Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
	Task<PagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterPrice);
	Task<IEnumerable<Product>> GetProductByCategoryAsync(int id);
}
