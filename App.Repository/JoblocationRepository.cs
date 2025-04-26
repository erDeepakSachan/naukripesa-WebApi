using App.Entity;

namespace App.Repository;

public class JoblocationRepository: GenericRepository<Joblocation>
{
    public JoblocationRepository(AppDbContext db) : base(db)
    {
    }
}