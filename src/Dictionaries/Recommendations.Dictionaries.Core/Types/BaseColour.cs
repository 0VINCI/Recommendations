namespace Recommendations.Dictionaries.Core.Types;

public sealed class BaseColour
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public BaseColour(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Id = id;
        Name = name;
    }

    public static BaseColour Create(string name)
    {
        return new BaseColour(Guid.NewGuid(), name);
    }

    public ICollection<Product> Products { get; set; } = new List<Product>();
} 