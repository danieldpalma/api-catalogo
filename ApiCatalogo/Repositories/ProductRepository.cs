using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
	public ProductRepository(AppDbContext context) : base(context) { }

	public async Task<IEnumerable<Product>> GetProductByCategoryAsync(int id)
	{
		var products = await GetAllAsync();
		var productsCategory = products.Where(c => c.CategoryId == id);
		return productsCategory;
	}

	//public IEnumerable<Product> GetProducts(ProductsParameters productsParameters)
	//{
	//	return GetAll()
	//		.OrderBy(p => p.Name)
	//		.Skip((productsParameters.PageNumber - 1) * productsParameters.PageSize)
	//		.Take(productsParameters.PageSize)
	//		.ToList();
	//}

	public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParameters)
	{
		var products = await GetAllAsync();
		var orderedProducts = products.OrderBy(p => p.ProductId).AsQueryable();
		//var result = PagedList<Product>.ToPagedList(orderedProducts, productsParameters.PageNumber, productsParameters.PageSize);
		var result = await orderedProducts.ToPagedListAsync(productsParameters.PageNumber, productsParameters.PageSize);

		return result;
	}

	public async Task<IPagedList<Product>> GetProductsFilterPriceAsync(ProductsFilterPrice productsFilterParams)
	{
		var products = await GetAllAsync();

		if (productsFilterParams.Price.HasValue && !string.IsNullOrEmpty(productsFilterParams.PriceCriterion))
		{
			if (productsFilterParams.PriceCriterion.Equals("maior", StringComparison.OrdinalIgnoreCase))
			{
				products = products.Where(p => p.Price > productsFilterParams.Price.Value).OrderBy(p => p.Price);
			}
			else if (productsFilterParams.PriceCriterion.Equals("menor", StringComparison.OrdinalIgnoreCase))
			{
				products = products.Where(p => p.Price < productsFilterParams.Price.Value).OrderBy(p => p.Price);
			}
			else if (productsFilterParams.PriceCriterion.Equals("igual", StringComparison.OrdinalIgnoreCase))
			{
				products = products.Where(p => p.Price == productsFilterParams.Price.Value).OrderBy(p => p.Price);
			}
		}

		//var filteredProducts = PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterParams.PageNumber, productsFilterParams.PageSize);
		var filteredProducts = await products.ToPagedListAsync(productsFilterParams.PageNumber, productsFilterParams.PageSize);
		return filteredProducts;
	}
}
