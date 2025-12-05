using ApiCatalogo.Models;
using ApiCatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IProductRepository _repository;

	public ProductsController(IProductRepository repository) => _repository = repository;

	[HttpGet]
	public ActionResult<IEnumerable<Product[]>> Get()
	{
		var products = _repository.GetProducts().ToList();

		return Ok(products);
	}

	[HttpGet("{id:int:min(1)}", Name = "GetProduct")]
	public ActionResult<Product> GetById(int id)
	{
		var product = _repository.GetProductById(id);

		if (product is null)
			return NotFound();

		return Ok(product);
	}

	[HttpPost]
	public ActionResult<Product> Post(Product product)
	{
		if(product is null)
			return BadRequest();

		var newProduct = _repository.CreateProduct(product);
		return new CreatedAtRouteResult("GetProduct", new { id = newProduct.ProductId }, newProduct);

	}

	[HttpPut("{id:int:min(1)}")]
	public ActionResult Update(int id, Product product)
	{
		if (product is null || id != product.ProductId)
			return BadRequest();

		bool result = _repository.UpdateProduct(product);
		if (result)
			return Ok(product);
		else
			return StatusCode(500, $"An error occurred while updating product with id {id}");
	}

	[HttpDelete("{id:int:min(1)}")]
	public ActionResult Delete(int id)
	{
		bool result = _repository.DeleteProduct(id);

		if (result is false)
			return StatusCode(500, $"An error occurred while deleting product with id {id}");

		return Ok();
	}
}
