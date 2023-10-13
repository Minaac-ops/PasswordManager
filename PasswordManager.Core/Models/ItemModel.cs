namespace PasswordManager.Core.Models;

public class ItemModel
{
    public string ItemName { get; set; }
    public string Username { get; set; }
    public byte[] EncryptedPassword { get; set; }
    public string UserId { get; set; }
}