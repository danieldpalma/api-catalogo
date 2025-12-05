using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly ICategoryRepository _repository;
	private readonly ILogger<CategoryController> _logger;

	public CategoryController(ICategoryRepository repository, ILogger<CategoryController> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	[HttpGet]
	public ActionResult<Category[]> Get()
	{
		var categories = _repository.GetCategories();
		if (categories is null)
			return NotFound();

		return Ok(categories);
	}

	[HttpGet("products")]
	public ActionResult<Category[]> GetWithProducts()
	{
		var categories = _repository.GetCategoriesWithProducts();

		if (categories is null)
			return NotFound();

		return Ok(categories);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetById")]
	public ActionResult<Category> GetById(int id)
	{
		var category = _repository.GetCategoryById(id);

		if (category is null)
		{
			_logger.LogWarning(message: $"Category id={id} not found.");
			return NotFound();
		}

		return Ok(category);
	}


	[HttpPost]
	public ActionResult<Category> Post(Category category)
	{
		if (category is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		var newCateogry = _repository.CreateCategory(category);

		return new CreatedAtRouteResult("GetById", new { id = newCateogry.CategoryId }, newCateogry);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<Category> Put(int id, Category category)
	{
		if (id != category.CategoryId)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		_repository.UpdateCategory(id, category);

		return Ok(category);
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult Delete(int id)
	{
		var category = _repository.GetCategoryById(id);
		if (category is null)
			return NotFound();

		_repository.DeleteCategory(id);

		return Ok();
	}
}
