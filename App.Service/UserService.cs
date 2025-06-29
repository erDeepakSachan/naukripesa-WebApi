using App.Entity;
using App.Repository;

namespace App.Service;

public class UserService : GenericService<User>
{
    public UserService(UserRepository repository) : base(repository)
    {
    }

    public User GetUserByEmailAndMobile(string email, string mobile)
    {
        return GetAll().FirstOrDefault(p => p.Email == email && p.MobileNo == mobile && p.IsArchived == false);
    }

    public User? Authenticate(string email, string password)
    {
        return GetAll().FirstOrDefault(p => p.Email == email && p.Password == password && p.IsArchived == true);
    }
}