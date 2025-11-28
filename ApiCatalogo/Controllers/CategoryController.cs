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

	public CategoryController(AppDbContext context) => _context = context;

	[HttpGet]
	public ActionResult<Category[]> GetCategories()
	{
		try
		{
			var categories = _context.Categories.AsNoTracking().ToArray();

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
	public ActionResult<Category[]> GetCategoriesWithProducts()
	{
		try
		{
			var categories = _context.Categories.AsNoTracking().Include(prod => prod.Products).ToArray();

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

	[HttpGet("{id:int}", Name = "GetCategoryById")]
	public ActionResult<Category> GetCategoryById(int id)
	{
		try
		{
			var category = _context.Categories.AsNoTracking().FirstOrDefault(prod => prod.CategoryId == id);

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

	[HttpPut("{id:int}")]
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

	[HttpDelete("{id:int}")]
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
