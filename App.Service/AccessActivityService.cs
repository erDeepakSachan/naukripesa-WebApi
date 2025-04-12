using App.Entity;
using App.Repository;

namespace App.Service;

public class AccessActivityService : GenericService<AccessActivity>
{
    public AccessActivityService(AccessActivityRepository repository) : base(repository)
    {
    }
}