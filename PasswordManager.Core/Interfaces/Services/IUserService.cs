using PasswordManager.Core.Models;

namespace PasswordManager.Core.Interfaces.Services;

public interface IUserService
{
    IEnumerable<UserModel> GetAllUsers();
    public void CreateUser(UserModel user);
}