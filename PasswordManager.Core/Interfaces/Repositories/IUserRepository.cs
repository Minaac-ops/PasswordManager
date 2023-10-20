using PasswordManager.Core.Models;

namespace PasswordManager.Core.Interfaces.Repositories;

public interface IUserRepository
{
    IEnumerable<UserModel> GetAllUsers();
    void CreateUser(UserModel user);
}