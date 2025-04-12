using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class DemoTabService : GenericService<DemoTab>,IDdlOption
{
    public DemoTabService(DemoTabRepository repository) : base(repository)
    {
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.DemoId)
        }).ToList();
    }
}