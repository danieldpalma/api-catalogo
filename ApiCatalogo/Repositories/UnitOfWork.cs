using ApiCatalogo.Context;

namespace ApiCatalogo.Repositories;

public class UnitOfWork : IUnitOfWork
{
	public AppDbContext _context;

	private IProductRepository? _productRepo;

	private ICategoryRepository? _categoryRepo;

	public UnitOfWork(AppDbContext context)
	{
		_context = context;
	}

	public IProductRepository ProductRepository
	{
		get { return _productRepo = _productRepo ?? new ProductRepository(_context); }
		set { _productRepo = value; }
	}

	public ICategoryRepository CategoryRepository
	{
		get { return _categoryRepo = _categoryRepo ?? new CategoryRepository(_context); }
		set { _categoryRepo = value; }
	}

	public async Task CommitAsync() => await _context.SaveChangesAsync();

	public void Dispose() => _context.Dispose();
}
