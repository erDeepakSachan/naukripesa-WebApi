using App.Entity;

namespace App.Repository;

public class ErrorActivityRepository: GenericRepository<ErrorActivity>
{
    public ErrorActivityRepository(AppDbContext db) : base(db)
    {
    }
}