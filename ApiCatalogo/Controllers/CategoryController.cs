using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

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
	[Authorize]
	public async Task<ActionResult<CategoryDTO[]>> Get()
	{
		var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
		if (categories is null)
			return NotFound();

		var categoriesDto = categories.ToCategoryDTOList();

		return Ok(categoriesDto);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetById")]
	public async Task<ActionResult<CategoryDTO>> GetById(int id)
	{
		var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);

		if (category is null)
		{
			_logger.LogWarning(message: $"Category id={id} not found.");
			return NotFound();
		}

		var categoryDTO = category.ToCategoryDTO();

		return Ok(categoryDTO);
	}

	[HttpGet("pagination")]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCategoriesWithPagination([FromQuery] CategoriesParameters categoriesParameters)
	{
		var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(categoriesParameters);
		return ObtainCategories(categories);
	}

	[HttpGet("filter/name/pagination")]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCategoriesFilteredName([FromQuery] CategoriesFilterName categoriesFilterName)
	{
		var filteredCategories = await _unitOfWork.CategoryRepository.GetCategoriesFilterNameAsync(categoriesFilterName);
		return ObtainCategories(filteredCategories);
	}

	[HttpPost]
	public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
	{
		if (categoryDto is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		var category = categoryDto.ToCategory();

		var newCateogry = _unitOfWork.CategoryRepository.Create(category!);
		await _unitOfWork.CommitAsync();

		var newCategoryDTO = newCateogry.ToCategoryDTO();

		return new CreatedAtRouteResult("GetById", new { id = newCategoryDTO!.CategoryId }, newCategoryDTO);
	}

	[HttpPut("{id:int:min(1)}")]
	public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
	{
		if (id != categoryDto.CategoryId || categoryDto is null)
		{
			_logger.LogWarning($"Invalid category.");
			return BadRequest();
		}

		var category = categoryDto.ToCategory();

		_unitOfWork.CategoryRepository.Update(category!);
		await _unitOfWork.CommitAsync();

		var updatedCategoryDTO = category!.ToCategoryDTO();

		return Ok(updatedCategoryDTO);
	}

	[HttpDelete("{id:int:min(1)}")]
	[Authorize(Policy = "AdminOnly")]
	public async Task<ActionResult> Delete(int id)
	{
		var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == id);
		if (category is null)
			return NotFound();

		_unitOfWork.CategoryRepository.Delete(category);
		await _unitOfWork.CommitAsync();

		return NoContent();
	}

	private ActionResult<IEnumerable<ProductDTO>> ObtainCategories(IPagedList<Category> categories)
	{
		var metadata = new
		{
			categories.Count,
			categories.PageSize,
			categories.PageCount,
			categories.TotalItemCount,
			categories.HasNextPage,
			categories.HasPreviousPage,
		};

		Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

		var categoriesDto = categories.ToCategoryDTOList();

		return Ok(categoriesDto);
	}
}
