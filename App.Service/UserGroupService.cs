using App.Dto;
using App.Entity;
using App.Repository;

namespace App.Service;

public class UserGroupService : GenericService<UserGroup>,IDdlOption
{
    public UserGroupService(UserGroupRepository repository) : base(repository)
    {
    }
    
    public List<DdlOption> GetAllDdlOptions()
    {
        return GetAll().Select(P => new DdlOption()
        {
            Text = P.Name,
            Value = Convert.ToString(P.UserGroupId)
        }).ToList();
    }
}