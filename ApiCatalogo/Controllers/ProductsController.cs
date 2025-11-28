using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly AppDbContext _context;

	public ProductsController(AppDbContext context) => _context = context;

	[HttpGet]
	public ActionResult<IEnumerable<Product[]>> GetProducts()
	{
		var products = _context.Products.ToArray();
		if (products is null)
		{
			return NotFound();
		}

		return Ok(products);
	}

	[HttpGet("{id:int}", Name = "GetProduct")]
	public ActionResult<Product> GetProductById(int id)
	{
		var product = _context.Products.FirstOrDefault(prod => prod.ProductId == id);
		if (product is null)
		{
			return NotFound();
		}

		return Ok(product);
	}

	[HttpPost]
	public ActionResult<Product> CreateProduct(Product product)
	{
		if (product is null)
		{
			return BadRequest();
		}

		_context.Products.Add(product);
		_context.SaveChanges();

		return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
	}

	[HttpPut("{id:int}")]
	public ActionResult UpdateProduct(int id, Product product)
	{
		if(id != product.ProductId || product is null)
		{
			return BadRequest();
		}

		_context.Entry(product).State = EntityState.Modified;
		_context.SaveChanges();

		return Ok(product);
	}

	[HttpDelete("{id:int}")]
	public ActionResult DeleteProduct(int id)
	{
		var product = _context.Products.FirstOrDefault(prod => prod.ProductId == id);

		if(product is null)
		{
			return NotFound(); 
		}

		_context.Products.Remove(product);
		_context.SaveChanges();

		return Ok();
	}
}
