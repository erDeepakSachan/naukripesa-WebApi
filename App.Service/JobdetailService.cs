using App.Entity;
using App.Repository;

namespace App.Service;

public class JobdetailService : GenericService<Jobdetail>
{
    public JobdetailService(JobdetailRepository repository) : base(repository)
    {
    }
}