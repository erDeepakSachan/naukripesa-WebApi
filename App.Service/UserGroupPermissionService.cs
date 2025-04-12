using App.Entity;
using App.Repository;

namespace App.Service;

public class UserGroupPermissionService : GenericService<UserGroupPermission>
{
    private readonly UserGroupPermissionRepository repository;

    public UserGroupPermissionService(UserGroupPermissionRepository repository) : base(repository)
    {
        this.repository = repository;
    }

    public List<dynamic> GetByUserGroup(int userGroupId)
    {
        return repository.GetByUserGroup(userGroupId);
    }

    public List<dynamic> GetDistinctMenuCategory(int userGroupId)
    {
        return repository.GetDistinctMenuCategory(userGroupId);
    }

    public List<dynamic> GetPermissionByParentWebpage(int userGroupId, int parentWebpageId)
    {
        return repository.GetPermissionByParentWebpage(userGroupId, parentWebpageId);
    }
}