using App.Entity;
using Microsoft.EntityFrameworkCore;

namespace App.Repository;

public class AccessActivityRepository : GenericRepository<AccessActivity>
{
    public AccessActivityRepository(AppDbContext db) : base(db)
    {
    }
}