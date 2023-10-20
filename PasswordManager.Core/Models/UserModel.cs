using System.Text.RegularExpressions;

namespace PasswordManager.Core.Models;

public partial class UserModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public byte[] EncryptedRandom { get; set; }
    public byte[] IV { get; set; }
    public byte[] PasswordSalt { get; set; }

    public bool IsPasswordValid()
    {
        return Password.Length > 8 &&
               Password.Any(char.IsUpper) &&
               Password.Any(char.IsLower) &&
               Password.Any(char.IsDigit) &&
               Password.Any(ch => "!@#$%^&*()".Contains(ch));
    }
}