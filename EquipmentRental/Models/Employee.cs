namespace EquipmentRental.Models;

public class Employee : User
{
    public Employee(string firstName, string lastName) : base(firstName, lastName)
    {
        MaxRentals = 5;
    }
}

