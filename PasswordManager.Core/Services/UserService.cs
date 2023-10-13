using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }
    
    public IEnumerable<UserModel> GetAllUsers()
    {
        return _repo.GetAllUsers();
    }

    public UserModel GetUser(string email)
    {
        return _repo.GetUser(email);
    }
    
    public void CreateUser(UserModel user)
    {
        if (user == null)
        {
            throw new ArgumentException();
        }

        _repo.CreateUser(user);
    }
}