
using JobTaskInpress.db;

namespace NewProject.Repositories;

public class UnitOfWork: IUnitOfWork, IDisposable
{
    private readonly MainDbContext _context;
    private readonly ILogger _logger;
    public UnitOfWork(MainDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger("logs");
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync(); 
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
