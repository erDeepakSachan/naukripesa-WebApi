using App.Entity;

namespace App.Repository;

public class SettingRepository : GenericRepository<Setting>
{
    public SettingRepository(AppDbContext db) : base(db)
    {
    }
}