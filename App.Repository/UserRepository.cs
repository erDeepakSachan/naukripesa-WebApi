using App.Entity;

namespace App.Repository;

public class UserRepository: GenericRepository<User>
{
    public UserRepository(AppDbContext db) : base(db)
    {
    }
}