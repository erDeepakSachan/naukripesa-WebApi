using App.Entity;

namespace App.Repository;

public class CurrencyRepository: GenericRepository<Currency>
{
    public CurrencyRepository(AppDbContext db) : base(db)
    {
    }
}