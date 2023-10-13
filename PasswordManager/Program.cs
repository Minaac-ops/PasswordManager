
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Repositories;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Services;
using PasswordManager.Infrastructure.Repositories;

namespace PasswordManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            
            services
                .AddSingleton<IAuthService, AuthService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IVaultService, VaultService>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IVaultRepository, VaultRepository>()
                .AddSingleton<IPasswordRepository, PasswordRepository>()
                .AddSingleton<PasswordManager, PasswordManager>()
                .BuildServiceProvider().GetService<PasswordManager>();
        }
    }
}