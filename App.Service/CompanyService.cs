using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class CompanyService : GenericService<Company>, IDdlOption
{
    public CompanyService(CompanyRepository repository) : base(repository)
    {
    }

    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.CompanyId)
        }).ToList();
    }
}