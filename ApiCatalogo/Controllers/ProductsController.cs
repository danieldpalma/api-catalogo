using ApiCatalogo.Extensions;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	//private readonly IProductRepository _repository;
	//private readonly IRepository<Product> _repository;

	public ProductsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

	[HttpGet]
	public ActionResult<IEnumerable<Product[]>> Get()
	{
		var products = _unitOfWork.ProductRepository.GetAll().ToList();

		return Ok(products);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetProduct")]
	public ActionResult<Product> GetById(int id)
	{
		var product = _unitOfWork.ProductRepository.Get(p => p.ProductId == id);

		if (product is null)
			return NotFound();

		return Ok(product);
	}

	[HttpGet("/ByCategory/{id:int:min(1)}", Name = "GetProductByCategory")]
	public ActionResult<IEnumerable<Product[]>> GetProductByCategory(int id)
	{
		var products = _unitOfWork.ProductRepository.GetProductByCategory(id);
		if (products.IsNullOrEmpty()) 
			return NotFound();

		return Ok(products);
	}

	[HttpPost]
	public ActionResult<Product> Post(Product product)
	{
		if(product is null)
			return BadRequest();

		var newProduct = _unitOfWork.ProductRepository.Create(product);
		_unitOfWork.Commit();

		return new CreatedAtRouteResult("GetProduct", new { id = newProduct.ProductId }, newProduct);
	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult Update(int id, Product product)
	{
		if (product is null || id != product.ProductId)
			return BadRequest();

		var updatedProduct = _unitOfWork.ProductRepository.Update(product);
		_unitOfWork.Commit();

		return Ok(updatedProduct);
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
