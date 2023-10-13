using PasswordManager.Core.Models;

namespace PasswordManager.Core.Interfaces.Repositories;

public interface IUserRepository
{
    IEnumerable<UserModel> GetAllUsers();
    UserModel GetUser(string email);
    void CreateUser(UserModel user);
}