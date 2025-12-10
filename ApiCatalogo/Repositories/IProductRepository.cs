using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
	IEnumerable<Product> GetProducts(ProductsParameters productsParameters);
	IEnumerable<Product> GetProductByCategory(int id);
}
