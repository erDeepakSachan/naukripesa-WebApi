using App.Entity;
using App.Repository;

namespace App.Service;

public class contactusService : GenericService<contactus>
{
    public contactusService(contactusRepository repository) : base(repository)
    {
    }
}