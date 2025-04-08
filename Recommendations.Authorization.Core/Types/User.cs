namespace Recommendations.Authorization.Core.Types;

public class User
{
    public Guid IdUser { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}