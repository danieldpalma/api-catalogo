using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
	PagedList<Product> GetProducts(ProductsParameters productsParameters);
	PagedList<Product> GetProductsFilterPrice(ProductsFilterPrice productsFilterPrice);
	IEnumerable<Product> GetProductByCategory(int id);
}
