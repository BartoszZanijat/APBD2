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

    public bool ReturnEquipment(string rentalId)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental == null || rental.ReturnDate != null)
            return false;

        rental.ReturnDate = DateTime.Now;
        if (rental.ReturnDate > rental.DueDate)
        {
            int daysLate = (rental.ReturnDate.Value - rental.DueDate).Days;
            rental.Fine = daysLate * 10m; // 10 PLN per day
        }
        else
        {
            rental.Fine = 0;
        }

        // Make equipment available again
        var equipment = rental.Equipment;
        equipment.IsAvailable = true;
        return true;
    }

    public List<Rental> GetActiveRentals() =>
        _rentals.Where(r => r.ReturnDate == null).ToList();

    public List<Rental> GetRentalsForUser(string userId) =>
        _rentals.Where(r => r.User.Id == userId).ToList();

    public bool MarkEquipmentAsDamaged(string equipmentId)
    {
        var eq = _equipment.FirstOrDefault(e => e.Id == equipmentId);
        if (eq == null) return false;
        eq.IsDamaged = true;
        eq.IsAvailable = false; // damaged equipment cannot be rented
        return true;
    }

    public List<Rental> GetOverdueRentals()
    {
        return _rentals.Where(r => r.ReturnDate == null && r.DueDate < DateTime.Now).ToList();
    }

    public string GenerateReport()
    {
        var totalEquipment = _equipment.Count;
        var availableCount = _equipment.Count(e => e.IsAvailable && !e.IsDamaged);
        var damagedCount = _equipment.Count(e => e.IsDamaged);
        var activeRentals = _rentals.Count(r => r.ReturnDate == null);
        var overdueCount = GetOverdueRentals().Count;
        var totalFinesCollected = _rentals.Where(r => r.Fine.HasValue).Sum(r => r.Fine!.Value);

        return $"""
                === Rental System Report ===
                Total equipment: {totalEquipment}
                Available: {availableCount}
                Damaged: {damagedCount}
                Active rentals: {activeRentals}
                Overdue rentals: {overdueCount}
                Total fines collected: {totalFinesCollected:C}
                """;
    }
}

