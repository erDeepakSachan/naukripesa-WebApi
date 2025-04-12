using App.Entity;

namespace App.Repository;

public class UserSessionRepository: GenericRepository<UserSession>
{
    public UserSessionRepository(AppDbContext db) : base(db)
    {
    }
}