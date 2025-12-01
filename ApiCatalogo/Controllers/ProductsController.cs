using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly AppDbContext _context;

	public ProductsController(AppDbContext context) => _context = context;

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product[]>>> GetProductsAsync()
	{
		try
		{
			var products = await _context.Products.AsNoTracking().ToArrayAsync();
			if (products is null)
			{
				return NotFound();
			}

			return Ok(products);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpGet("{id:int:min(1)}", Name = "GetProduct")]
	public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
	{
		try
		{
			var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(prod => prod.ProductId == id);
			if (product is null)
			{
				return NotFound();
			}

			return Ok(product);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpPost]
	public ActionResult<Product> CreateProduct(Product product)
	{
		try
		{
			if (product is null)
			{
				return BadRequest();
			}

			_context.Products.Add(product);
			_context.SaveChanges();

			return new CreatedAtRouteResult("GetProduct", new { id = product.ProductId }, product);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult UpdateProduct(int id, Product product)
	{
		try
		{
			if (id != product.ProductId || product is null)
			{
				return BadRequest();
			}

			_context.Entry(product).State = EntityState.Modified;
			_context.SaveChanges();

			return Ok(product);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult DeleteProduct(int id)
	{
		try
		{
			var product = _context.Products.FirstOrDefault(prod => prod.ProductId == id);

			if (product is null)
			{
				return NotFound();
			}

			_context.Products.Remove(product);
			_context.SaveChanges();

			return Ok();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}
}
