// See https://aka.ms/new-console-template for more information

using System.Text;
using PasswordManager.Core.Interfaces;
using PasswordManager.Core.Interfaces.Services;
using PasswordManager.Core.Models;
using ZstdSharp.Unsafe;

namespace PasswordManager
{
    public class PasswordManager
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IVaultService _vaultService;
        private UserModel _loggedInUser;
        
        public PasswordManager(IAuthService authService,
            IUserService userService,
            IVaultService vaultService)
        {
            _authService = authService;
            _userService = userService;
            _vaultService = vaultService;
            Welcome();
        }

        private void Welcome()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Password Manager. Your options are:" +
                              "\n1: New user? | 2: Login");
            
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    Login();
                    break;
            }
        }
        
        private void Register()
        {
            Console.WriteLine("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Master password: ");
            var password = ReadPassword();
            
            try
            {
                var newUser = new UserModel
                {
                    Username = username!,
                    Password = password!
                };
                if (newUser.IsPasswordValid())
                {
                    var user = _authService.PasswordHasher(username!, password!);
        
                    _userService.CreateUser(user);
                    Thread.Sleep(3000);
                    Welcome();
                }
                else
                {
                    Console.WriteLine(
                        "Password must be at least 8 characters and must contain small and capital letters, digits and at least one special character.");
                    Thread.Sleep(6000);
                    Register();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Please right a valid username and password");
                throw;
            }
        }

        private void Login()
        {
            Console.WriteLine("Username: ");
            var username = Console.ReadLine();
        
            Console.WriteLine("Password: ");
            var pass = ReadPassword();
            try
            {
                var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);
                if (user == null) Console.WriteLine("Username or password incorrect");
                var authenticated = _authService.AuthenticateLogin(user!, pass!);
                if (!authenticated) Console.WriteLine("Username or password incorrect");
                
                else
                {
                    _loggedInUser = user!;
                    Menu();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Username or password incorrect");
                throw;
            }
        }

        private void Menu()
        {
            Console.Clear();
            Console.WriteLine("Press...\n1: to view your items\n2: to create a new item\n3: to exit application");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ViewItems();
                    break;
                case "2":
                    CreateItem();
                    break;
                case "3":
                    Console.Clear();
                    Environment.Exit(0);
                    break;
            }
        }

        private void CreateItem()
        {
            var password ="";
            Console.WriteLine("Name of new item:");
            var itemName = Console.ReadLine();
            Console.WriteLine("Username:");
            var username = Console.ReadLine();
            Console.WriteLine("Do you want PasswordManager to generate a secure password for you? Y/N");
            var answer = Console.ReadLine()?.Trim().ToLower();
            switch (answer)
            {
                case "y" or "yes":
                    password = GeneratePassword();
                    break;
                case "n" or "no":
                {
                    Console.WriteLine("Type in your password");
                    password = ReadPassword();
                    break;
                }
            }
            
            var newItem = new ItemModel
            {
                ItemName = itemName!,
                Username = username!,
                UserId = _loggedInUser.Id,
            };
            _authService.EncryptItemPassword(newItem, password);
            
            Console.WriteLine("Your new item is saved to your vault.");
            Thread.Sleep(3);
            Menu();
        }

        private void ViewItems()
        {
            var items = _vaultService.GetItemsByUserId(_loggedInUser.Id).ToList();
            foreach (var item in items)
            {
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine($"║        {item.ItemName}          ║");
                Console.WriteLine("╠══════════════════════════════════╣");
                Console.WriteLine($"║ Username : {item.Username,-24} ║");
                Console.WriteLine($"║ Password : {_authService.GetDecryptedPassword(item),-24} ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.WriteLine();
            }
            Console.WriteLine("1 to go back to main menu or 2 to exit application");

            switch (Console.ReadLine())
            {
                case "1":
                    Menu();
                    break;
                case "2":
                    Console.Clear();
                    Environment.Exit(0);
                    break;
            }
        }
        
        private static string GeneratePassword()
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789$%&!";
            var pass = new StringBuilder();

            while (pass.Length < 12)
            {
                var index = random.Next(chars.Length);
                var randomchar = chars[index];

                if (!pass.ToString().Contains(randomchar.ToString()))
                {
                    pass.Append(randomchar);
                }
            }

            return pass.ToString();
        }

        private static string ReadPassword()
        {
            var password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                } else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            return password;
        }
    }
}

