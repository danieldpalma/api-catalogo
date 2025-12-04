using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly AppDbContext _context;
	private readonly ILogger<CategoryController> _logger;

	public CategoryController(AppDbContext context, ILogger<CategoryController> logger)
	{
		_context = context;
		_logger = logger;
	}

	[HttpGet]
	public async Task<ActionResult<Category[]>> GetCategoriesAsync()
	{
		var categories = await _context.Categories.AsNoTracking().ToArrayAsync();

		if (categories is null)
		{
			return NotFound();
		}

		return Ok(categories);
	}

	[HttpGet("products")]
	public async Task<ActionResult<Category[]>> GetCategoriesWithProductsAsync()
	{
		var categories = await _context.Categories.AsNoTracking().Include(prod => prod.Products).ToArrayAsync();

		if (categories is null)
		{
			return NotFound();
		}

		return Ok(categories);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetCategoryById")]
	public async Task<ActionResult<Category>> GetCategoryByIdAsync(int id)
	{
		var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(prod => prod.CategoryId == id);

		if (category is null)
		{
			_logger.LogWarning(message: $"Category id={id} not found.");
			return NotFound();
		}

		return Ok(category);
	}


	[HttpPost]
	public ActionResult<Category> CreateCategory(Category category)
	{
		if (category is null)
		{
			return BadRequest();
		}

		_context.Categories.Add(category);
		_context.SaveChanges();

		return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<Category> UpdateCategory(int id, Category category)
	{
		if (id != category.CategoryId)
		{
			return NotFound();
		}

		_context.Categories.Entry(category).State = EntityState.Modified;
		_context.SaveChanges();

		return Ok(category);
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult DeleteCategory(int id)
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
}
