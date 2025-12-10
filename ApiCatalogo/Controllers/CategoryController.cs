using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<CategoryController> _logger;

	public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	[HttpGet]
	public ActionResult<Category[]> Get()
	{
		var categories = _unitOfWork.CategoryRepository.GetAll();
		if (categories is null)
			return NotFound();

		return Ok(categories);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetById")]
	public ActionResult<Category> GetById(int id)
	{
		var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

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

		var newCateogry = _unitOfWork.CategoryRepository.Create(category);
		_unitOfWork.Commit();

		return new CreatedAtRouteResult("GetById", new { id = newCateogry.CategoryId }, newCateogry);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<Category> Put(int id, Category category)
	{
		if (id != category.CategoryId || category is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		_unitOfWork.CategoryRepository.Update(category);
		_unitOfWork.Commit();

		return Ok(category);
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult Delete(int id)
	{
		var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);
		if (category is null)
			return NotFound();

		_unitOfWork.CategoryRepository.Delete(category);
		_unitOfWork.Commit();

		return Ok();
	}
}
