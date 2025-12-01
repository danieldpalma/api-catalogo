using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly AppDbContext _context;

	public CategoryController(AppDbContext context) => _context = context;

	[HttpGet]
	public async Task<ActionResult<Category[]>> GetCategoriesAsync()
	{
		try
		{
			var categories = await _context.Categories.AsNoTracking().ToArrayAsync();

			if (categories is null)
			{
				return NotFound();
			}

			return Ok(categories);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpGet("products")]
	public async Task<ActionResult<Category[]>> GetCategoriesWithProductsAsync()
	{
		try
		{
			var categories = await _context.Categories.AsNoTracking().Include(prod => prod.Products).ToArrayAsync();

			if (categories is null)
			{
				return NotFound();
			}

			return Ok(categories);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
	public async Task<ActionResult<Category>> GetCategoryByIdAsync(int id)
	{
		try
		{
			var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(prod => prod.CategoryId == id);

			if (category is null)
			{
				return NotFound();
			}

			return Ok(category);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpPost]
	public ActionResult<Category> CreateCategory(Category category)
	{
		try
		{
			if (category is null)
			{
				return BadRequest();
			}

			_context.Categories.Add(category);
			_context.SaveChanges();

			return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<Category> UpdateCategory(int id, Category category)
	{
		try
		{
			if (id != category.CategoryId)
			{
				return NotFound();
			}

			_context.Categories.Entry(category).State = EntityState.Modified;
			_context.SaveChanges();

			return Ok(category);
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult DeleteCategory(int id)
	{
		try
		{
			var category = _context.Categories.FirstOrDefault(cat => cat.CategoryId == id);
			if (category is null)
			{
				return NotFound();
			}

			_context.Categories.Remove(category);
			_context.SaveChanges();

			return Ok();
		}
		catch (Exception)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
		}
	}
}
