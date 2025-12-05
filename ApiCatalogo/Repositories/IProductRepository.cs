using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public interface IProductRepository
{
	IQueryable<Product> GetProducts();
	Product GetProductById(int id);
	Product CreateProduct(Product product);
	bool UpdateProduct(Product product);
	bool DeleteProduct(int id);
}
