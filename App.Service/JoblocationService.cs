using App.Entity;
using App.Repository;

namespace App.Service;

public class JoblocationService : GenericService<Joblocation>
{
    public JoblocationService(JoblocationRepository repository) : base(repository)
    {
    }
}