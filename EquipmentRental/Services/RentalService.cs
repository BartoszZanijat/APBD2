using EquipmentRental.Models;

namespace EquipmentRental.Services;

public class RentalService
{
    private List<User> _users = new();
    private List<Equipment> _equipment = new();
    private List<Rental> _rentals = new();

    public void AddUser(User user) => _users.Add(user);
    public void AddEquipment(Equipment eq) => _equipment.Add(eq);

    public List<User> GetAllUsers() => _users;
    public List<Equipment> GetAllEquipment() => _equipment;

    // placeholder for later
    public List<Equipment> GetAvailableEquipment() =>
        _equipment.Where(e => e.IsAvailable && !e.IsDamaged).ToList();
}

