using App.Entity;

namespace App.Repository;

public class DemoTabRepository: GenericRepository<DemoTab>
{
    public DemoTabRepository(AppDbContext db) : base(db)
    {
    }
}