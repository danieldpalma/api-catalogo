using ApiCatalogo.DTOs;
using ApiCatalogo.Extensions;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	//private readonly IProductRepository _repository;
	//private readonly IRepository<Product> _repository;

	public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ProductDTO[]>>> Get()
	{
		var products = await _unitOfWork.ProductRepository.GetAllAsync();

		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetProduct")]
	public async Task<ActionResult<ProductDTO>> GetById(int id)
	{
		var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		var productDto = _mapper.Map<ProductDTO>(product);

		return Ok(productDto);
	}

	[HttpGet("/ByCategory/{id:int:min(1)}", Name = "GetProductByCategory")]
	public async Task<ActionResult<IEnumerable<ProductDTO[]>>> GetProductByCategory(int id)
	{
		var products = await _unitOfWork.ProductRepository.GetProductByCategoryAsync(id);
		if (products.IsNullOrEmpty())
			return NotFound();

		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}

	[HttpGet("pagination")]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsWithPagination([FromQuery] ProductsParameters productsParameters)
	{
		var products = await _unitOfWork.ProductRepository.GetProductsAsync(productsParameters);
		return ObtainProducts(products);
	}

	[HttpGet("filter/price/pagination")]
	public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsFilterPrice([FromQuery] ProductsFilterPrice productsFilterParams)
	{
		var products = await _unitOfWork.ProductRepository.GetProductsFilterPriceAsync(productsFilterParams);

		return ObtainProducts(products);
	}

	[HttpPost]
	public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
	{
		if (productDto is null)
			return BadRequest();

		var product = _mapper.Map<Product>(productDto);

		var newProduct = _unitOfWork.ProductRepository.Create(product);
		await _unitOfWork.CommitAsync();

		var newProductDto = _mapper.Map<Product>(newProduct);

		return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.ProductId }, newProductDto);
	}

	[HttpPatch("{id:int:min(1)}/updatePartial")]
	public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDto)
	{
		if (patchProductDto is null || id <= 0)
			return BadRequest();

		var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

		patchProductDto.ApplyTo(productUpdateRequest, ModelState);

		if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
			return BadRequest(ModelState);

		_mapper.Map(productUpdateRequest, product);
		await _unitOfWork.CommitAsync();

		var productResponse = _mapper.Map<ProductDTOUpdateResponse>(product);
		return Ok(productResponse);
	}

	[HttpPut("{id:int:min(1)}")]
	public async Task<ActionResult<ProductDTO>> Update(int id, ProductDTO productDto)
	{
		if (productDto is null || id != productDto.ProductId)
			return BadRequest();

		var product = _mapper.Map<Product>(productDto);

		var updatedProduct = _unitOfWork.ProductRepository.Update(product);
		await _unitOfWork.CommitAsync();

		var updatedProductDto = _mapper.Map<Product>(updatedProduct);

		return Ok(updatedProductDto);
	}

	[HttpDelete("{id:int:min(1)}")]
	public async Task<ActionResult> Delete(int id)
	{
		var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		_unitOfWork.ProductRepository.Delete(product);
		await _unitOfWork.CommitAsync();

		return Ok();
	}

	private ActionResult<IEnumerable<ProductDTO>> ObtainProducts(PagedList<Product> products)
	{
		var metadata = new
		{
			products.TotalCount,
			products.PageSize,
			products.CurrentPage,
			products.TotalPages,
			products.HasNext,
			products.HasPrevius,
		};

		Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}
}
