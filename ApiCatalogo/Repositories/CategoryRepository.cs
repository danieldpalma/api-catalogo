using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoryRepository : ICategoryRepository
{
	private readonly AppDbContext _context;

	public CategoryRepository(AppDbContext context) => _context = context;

	public IEnumerable<Category> GetCategories()
	{
		var categories = _context.Categories.AsNoTracking().ToArray();
		return categories;
	}

	public Category GetCategoryById(int id)
	{
		var category = _context.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
		return category;
	}

	public IEnumerable<Category> GetCategoriesWithProducts()
	{
		var categories = _context.Categories.AsNoTracking().Include(prod => prod.Products).ToArray();
		return categories;
	}

	public Category CreateCategory(Category category)
	{
		if (category is null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		_context.Categories.Add(category);
		_context.SaveChanges();
		return category;
	}

	public Category UpdateCategory(int id, Category category)
	{
		if (category is null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		_context.Categories.Entry(category).State = EntityState.Modified;
		_context.SaveChanges();
		return category;
	}

	public Category DeleteCategory(int id)
	{
		var category = _context.Categories.Find(id);

		if (category is null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		_context.Categories.Remove(category);
		_context.SaveChanges();

		return category;
	}
}
