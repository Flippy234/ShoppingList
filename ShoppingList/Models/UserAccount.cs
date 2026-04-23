namespace ShoppingList.Models;

public class UserAccount
{
    public string username { get; set; }
    public string password { get; set; }
    public string email { get; set; }

    public UserAccount(string username, string password,string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;

    }
}