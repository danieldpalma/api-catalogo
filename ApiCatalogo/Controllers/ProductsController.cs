using ApiCatalogo.DTOs;
using ApiCatalogo.Extensions;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
	public ActionResult<IEnumerable<ProductDTO[]>> Get()
	{
		var products = _unitOfWork.ProductRepository.GetAll().ToList();

		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetProduct")]
	public ActionResult<ProductDTO> GetById(int id)
	{
		var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		var productDto = _mapper.Map<ProductDTO>(product);

		return Ok(productDto);
	}

	[HttpGet("/ByCategory/{id:int:min(1)}", Name = "GetProductByCategory")]
	public ActionResult<IEnumerable<ProductDTO[]>> GetProductByCategory(int id)
	{
		var products = _unitOfWork.ProductRepository.GetProductByCategory(id);
		if (products.IsNullOrEmpty()) 
			return NotFound();

		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}

	[HttpGet("pagination")]
	public ActionResult<IEnumerable<ProductDTO>> GetProductsWithPagination([FromQuery] ProductsParameters productsParameters)
	{
		var products = _unitOfWork.ProductRepository.GetProducts(productsParameters);
		var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

		return Ok(productsDto);
	}

	[HttpPost]
	public ActionResult<ProductDTO> Post(ProductDTO productDto)
	{
		if(productDto is null)
			return BadRequest();

		var product = _mapper.Map<Product>(productDto);

		var newProduct = _unitOfWork.ProductRepository.Create(product);
		_unitOfWork.Commit();

		var newProductDto = _mapper.Map<Product>(newProduct);

		return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.ProductId }, newProductDto);
	}

	[HttpPatch("{id:int:min(1)}/updatePartial")]
	public ActionResult<ProductDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDto)
	{
		if (patchProductDto is null || id <= 0)
			return BadRequest();

		var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

		patchProductDto.ApplyTo(productUpdateRequest, ModelState);

		if (!ModelState.IsValid || !TryValidateModel(productUpdateRequest))
			return BadRequest(ModelState);

		_mapper.Map(productUpdateRequest, product);
		_unitOfWork.Commit();

		var productResponse = _mapper.Map<ProductDTOUpdateResponse>(product);
		return Ok(productResponse);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult<ProductDTO> Update(int id, ProductDTO productDto)
	{
		if (productDto is null || id != productDto.ProductId)
			return BadRequest();

		var product = _mapper.Map<Product>(productDto);

		var updatedProduct = _unitOfWork.ProductRepository.Update(product);
		_unitOfWork.Commit();

		var updatedProductDto = _mapper.Map<Product>(updatedProduct);

		return Ok(updatedProductDto);
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult Delete(int id)
	{
		var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		_unitOfWork.ProductRepository.Delete(product);
		_unitOfWork.Commit();

		return Ok();
	}
}
