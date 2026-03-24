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

    public bool RentEquipment(string userId, string equipmentId, int days)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        var equipment = _equipment.FirstOrDefault(e => e.Id == equipmentId);

        if (user == null || equipment == null)
            return false;

        // Check if equipment is available (not damaged and not rented)
        if (!equipment.IsAvailable || equipment.IsDamaged)
            return false;

        // Check user's active rentals count
        var activeRentalsCount = _rentals.Count(r => r.User.Id == userId && r.ReturnDate == null);
        if (activeRentalsCount >= user.MaxRentals)
            return false;

        var rentDate = DateTime.Now;
        var dueDate = rentDate.AddDays(days);
        var rental = new Rental(user, equipment, rentDate, dueDate);
        _rentals.Add(rental);
        equipment.IsAvailable = false; // mark as rented
        return true;
    }
}

