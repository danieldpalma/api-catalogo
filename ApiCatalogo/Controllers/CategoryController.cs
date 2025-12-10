using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

	[HttpGet("pagination")]
	public ActionResult<IEnumerable<ProductDTO>> GetCategoriesWithPagination([FromQuery] CategoriesParameters categoriesParameters)
	{
		var categories = _unitOfWork.CategoryRepository.GetCategories(categoriesParameters);
		return ObtainCategories(categories);
	}

	

	[HttpGet("filter/name/pagination")]
	public ActionResult<IEnumerable<ProductDTO>> GetCategoriesFilteredName([FromQuery] CategoriesFilterName categoriesFilterName)
	{
		var filteredCategories = _unitOfWork.CategoryRepository.GetCategoriesFilterName(categoriesFilterName);
		return ObtainCategories(filteredCategories);
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

	private ActionResult<IEnumerable<ProductDTO>> ObtainCategories(PagedList<Category> categories)
	{
		var metadata = new
		{
			categories.TotalCount,
			categories.PageSize,
			categories.CurrentPage,
			categories.TotalPages,
			categories.HasNext,
			categories.HasPrevius,
		};

		Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

		var categoriesDto = categories.ToCategoryDTOList();

		return Ok(categoriesDto);
	}
}
