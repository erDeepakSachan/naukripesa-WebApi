using App.Entity;

namespace App.Repository;

public class MenuCategoryRepository: GenericRepository<MenuCategory>
{
    public MenuCategoryRepository(AppDbContext db) : base(db)
    {
    }
}