using App.Entity;
using App.Repository;

namespace App.Service;

public class ErrorActivityService : GenericService<ErrorActivity>
{
    public ErrorActivityService(ErrorActivityRepository repository) : base(repository)
    {
    }
}