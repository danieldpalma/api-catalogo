using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class ProductRepository : IProductRepository
{
	private readonly AppDbContext _context;

	public ProductRepository(AppDbContext context) => _context = context;

	public IQueryable<Product> GetProducts()
	{
		return _context.Products;
	}

	public Product GetProductById(int id)
	{
		var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
		if (product is null)
			throw new InvalidOperationException("Invalid product.");

		return product;
	}

	public Product CreateProduct(Product product)
	{
		if (product is null)
			throw new InvalidOperationException("Invalid product.");

		_context.Products.Add(product);
		_context.SaveChanges();
		return product;
	}

	public bool UpdateProduct(Product product)
	{
		if (_context.Products.Any(p => p.ProductId == product.ProductId))
		{
			_context.Products.Update(product);
			_context.SaveChanges();
			return true;
		}

		return false;
	}

	public bool DeleteProduct(int id)
	{
		var product = _context.Products.Find(id);

		if (product is null)
			return false;

		_context.Products.Remove(product);
		_context.SaveChanges();

		return true;
	}
}
