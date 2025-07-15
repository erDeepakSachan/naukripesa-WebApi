using App.Entity;

namespace App.Repository;

public class contactusRepository: GenericRepository<contactus>
{
    public contactusRepository(AppDbContext db) : base(db)
    {
    }
}