using App.Entity;

namespace App.Repository;

public class UserTypeRepository: GenericRepository<UserType>
{
    public UserTypeRepository(AppDbContext db) : base(db)
    {
    }
}