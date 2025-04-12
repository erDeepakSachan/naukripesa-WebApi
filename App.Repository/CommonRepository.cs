using App.Entity;

namespace App.Repository;

public class CommonRepository
{
    private readonly AppDbContext db;

    public CommonRepository(AppDbContext db)
    {
        this.db = db;
    }

    public string GetSettingValueFromDb(string key)
    {
        return this.db.Set<Setting>().Where(p => p.Name == key).FirstOrDefault()?.Value;
    }
}