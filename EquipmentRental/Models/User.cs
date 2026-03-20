namespace EquipmentRental.Models;

public abstract class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int MaxRentals { get; protected set; }

    protected User(string firstName, string lastName)
    {
        Id = Guid.NewGuid().ToString().Substring(0, 8);
        FirstName = firstName;
        LastName = lastName;
    }
}

