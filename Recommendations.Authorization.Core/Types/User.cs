namespace Recommendations.Authorization.Core.Types;

public sealed class User(string name, string surname, string email, string password)
{
    public Guid IdUser { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public string Surname { get; private set; } = surname;
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    
    public static User Create(string name, string surname, string email, string password)
    {
        return new User(name, surname, email, password);
    }

    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
    }
}