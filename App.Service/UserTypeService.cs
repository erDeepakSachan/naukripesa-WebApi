using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class UserTypeService : GenericService<UserType>,IDdlOption
{
    public UserTypeService(UserTypeRepository repository) : base(repository)
    {
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.UserTypeId)
        }).ToList();
    }
}