using App.Entity;

namespace App.Repository;

public class CompanyRepository: GenericRepository<Company>
{
    public CompanyRepository(AppDbContext db) : base(db)
    {
    }
}