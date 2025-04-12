using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class AppIconService : GenericService<AppIcon>,IDdlOption
{
    public AppIconService(AppIconRepository repository) : base(repository)
    {
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.AppIconId),
            Data = @$"<i class='fa {P.CssClass}' style='color:{P.IconColor}'></i> {P.Name}"
        }).ToList();
    }
}