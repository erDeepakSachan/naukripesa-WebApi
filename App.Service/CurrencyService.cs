using App.Entity;
using App.Repository;

namespace App.Service;

public class CurrencyService : GenericService<Currency>
{
    public CurrencyService(CurrencyRepository repository) : base(repository)
    {
    }
}