using App.Entity;

namespace App.Repository;

public class ProductRepository: GenericRepository<Product>
{
    public ProductRepository(AppDbContext db) : base(db)
    {
    }
}