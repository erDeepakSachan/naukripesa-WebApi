using App.Entity;

namespace App.Repository;

public class JobdetailRepository: GenericRepository<Jobdetail>
{
    public JobdetailRepository(AppDbContext db) : base(db)
    {
    }
}