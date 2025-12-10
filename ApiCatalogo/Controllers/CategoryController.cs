using ApiCatalogo.DTOs;
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
	public ActionResult<CategoryDTO[]> Get()
	{
		var categories = _unitOfWork.CategoryRepository.GetAll();
		if (categories is null)
			return NotFound();

		var categoriesDto = new List<CategoryDTO>();

		foreach (var category in categories)
		{
			var categoryDto = new CategoryDTO()
			{
				CategoryId = category.CategoryId,
				Name = category.Name,
				ImageUrl = category.ImageUrl,
			};

			categoriesDto.Add(categoryDto);
		}

		return Ok(categoriesDto);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetById")]
	public ActionResult<CategoryDTO> GetById(int id)
	{
		var category = _unitOfWork.CategoryRepository.Get(c => c.CategoryId == id);

		if (category is null)
		{
			_logger.LogWarning(message: $"Category id={id} not found.");
			return NotFound();
		}

		var categoryDTO = new CategoryDTO()
		{
			CategoryId = category.CategoryId,
			Name = category.Name,
			ImageUrl = category.ImageUrl,
		};

		return Ok(categoryDTO);
	}


	[HttpPost]
	public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
	{
		if (categoryDto is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		var category = new Category()
		{
			CategoryId = categoryDto.CategoryId,
			Name = categoryDto.Name,
			ImageUrl = categoryDto.ImageUrl,
		};

		var newCateogry = _unitOfWork.CategoryRepository.Create(category);
		_unitOfWork.Commit();

		var newCategoryDTO = new CategoryDTO()
		{
			CategoryId = category.CategoryId,
			Name = category.Name,
			ImageUrl = category.ImageUrl,
		};

		return new CreatedAtRouteResult("GetById", new { id = newCategoryDTO.CategoryId }, newCategoryDTO);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
	{
		if (id != categoryDto.CategoryId || categoryDto is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		var category = new Category()
		{
			CategoryId = categoryDto.CategoryId,
			Name = categoryDto.Name,
			ImageUrl = categoryDto.ImageUrl,
		};

		_unitOfWork.CategoryRepository.Update(category);
		_unitOfWork.Commit();

		var newCategoryDTO = new CategoryDTO()
		{
			CategoryId = category.CategoryId,
			Name = category.Name,
			ImageUrl = category.ImageUrl,
		};

		return Ok(newCategoryDTO);
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
