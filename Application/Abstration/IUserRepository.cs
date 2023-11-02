using Domain;

namespace Application.Abstration;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    Task<User> AddUser(User user);
    User UpdateUser(User user);
    User GetUser(int userId);
    void DeleteUser(int userId);
}