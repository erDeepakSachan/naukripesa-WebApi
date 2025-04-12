using App.Entity;
using App.Repository;

namespace App.Service;

public class SettingService : GenericService<Setting>
{
    public SettingService(SettingRepository repository) : base(repository)
    {
    }
}