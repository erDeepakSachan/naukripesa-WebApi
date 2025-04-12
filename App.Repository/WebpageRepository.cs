using App.Entity;

namespace App.Repository;

public class WebpageRepository: GenericRepository<Webpage>
{
    public WebpageRepository(AppDbContext db) : base(db)
    {
    }
    
    public new IQueryable<Webpage> GetAll()
    {
        return table.Where(p => p.WebpageId > 0).AsQueryable();
    } 
}