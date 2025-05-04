using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class JoblocationService : GenericService<Joblocation>, IDdlOption
{
    public JoblocationService(JoblocationRepository repository) : base(repository)
    {
    }

    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Location,
            Value = Convert.ToString(P.JobLocationId)
        }).ToList();
    }
}