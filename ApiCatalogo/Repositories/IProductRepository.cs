using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public interface IProductRepository : IRepository<Product>
{
	IEnumerable<Product> GetProductByCategory(int id);
}
