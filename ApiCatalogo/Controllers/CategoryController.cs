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
		var categories = _context.Categories.ToArray();

		if (categories is null)
		{
			return NotFound();
		}

		return Ok(categories);
	}

	[HttpGet("products")]
	public ActionResult<Category[]> GetCategoriesWithProducts()
	{
		var categories = _context.Categories.Include(prod => prod.Products).ToArray();

		if (categories is null)
		{
			return NotFound();
		}

		return Ok(categories);
	}

	[HttpGet("{id:int}", Name = "GetCategoryById")]
	public ActionResult<Category> GetCategoryById(int id)
	{
		var category = _context.Categories.FirstOrDefault(prod => prod.CategoryId == id);

		if (category is null)
		{
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

	[HttpPut("{id:int}")]
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

	[HttpDelete("{id:int}")]
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
