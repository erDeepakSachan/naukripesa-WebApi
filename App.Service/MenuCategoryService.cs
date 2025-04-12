using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class MenuCategoryService : GenericService<MenuCategory>,IDdlOption
{
    public MenuCategoryService(MenuCategoryRepository repository) : base(repository)
    {
    }

    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.MenuCategoryId)
        }).ToList();
    }
}