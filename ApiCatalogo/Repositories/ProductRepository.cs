using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
	public ProductRepository(AppDbContext context) : base(context) { }

	public IEnumerable<Product> GetProductByCategory(int id)
	{
		return GetAll().Where(c => c.CategoryId == id);
	}

	//public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
	//{
	//	return GetAll()
	//		.OrderBy(p => p.Name)
	//		.Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
	//		.Take(productsParameters.PageSize)
	//		.ToList();
	//}

	public PagedList<Product> GetProducts(ProductsParameters productsParameters)
	{
		var products = GetAll().OrderBy(p => p.ProductId).AsQueryable();
		var orderedProducts = PagedList<Product>.ToPagedList(products, productsParameters.PageNumber, productsParameters.PageSize);

		return orderedProducts;
	}
}
