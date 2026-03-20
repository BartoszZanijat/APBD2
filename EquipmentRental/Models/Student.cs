namespace EquipmentRental.Models;

public class Student : User
{
    public Student(string firstName, string lastName) : base(firstName, lastName)
    {
        MaxRentals = 2;
    }
}

