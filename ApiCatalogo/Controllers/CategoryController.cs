using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
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

		var categoriesDto = categories.ToCategoryDTOList();

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

		var categoryDTO = category.ToCategoryDTO();

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

		var category = categoryDto.ToCategory();

		var newCateogry = _unitOfWork.CategoryRepository.Create(category);
		_unitOfWork.Commit();

		var newCategoryDTO = newCateogry.ToCategoryDTO();

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

		var category = categoryDto.ToCategory();

		_unitOfWork.CategoryRepository.Update(category);
		_unitOfWork.Commit();

		var updatedCategoryDTO = category.ToCategoryDTO();

		return Ok(updatedCategoryDTO);
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
