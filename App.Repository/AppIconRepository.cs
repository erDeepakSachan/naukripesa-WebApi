using App.Entity;

namespace App.Repository;

public class AppIconRepository: GenericRepository<AppIcon>
{
    public AppIconRepository(AppDbContext db) : base(db)
    {
    }
}