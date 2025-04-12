using App.Entity;

namespace App.Repository;

public class UserGroupRepository: GenericRepository<UserGroup>
{
    public UserGroupRepository(AppDbContext db) : base(db)
    {
    }
}