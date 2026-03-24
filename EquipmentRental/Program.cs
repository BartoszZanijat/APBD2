using System.Linq;
using EquipmentRental.Models;
using EquipmentRental.Services;

namespace EquipmentRental;

class Program
{
    private static readonly RentalService service = new();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            DisplayMenu();

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    AddEquipment();
                    break;
                case "3":
                    ListAllEquipment();
                    break;
                case "4":
                    ListAvailableEquipment();
                    break;
                case "5":
                    RentEquipment();
                    break;
                case "6":
                    ReturnEquipment();
                    break;
                case "7":
                    MarkEquipmentDamaged();
                    break;
                case "8":
                    ShowUserRentals();
                    break;
                case "9":
                    ShowOverdueRentals();
                    break;
                case "10":
                    ShowActiveRentals();
                    break;
                case "11":
                    ShowReport();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("=== Equipment Rental System ===");
        Console.WriteLine("1. Add user");
        Console.WriteLine("2. Add equipment");
        Console.WriteLine("3. List all equipment");
        Console.WriteLine("4. List available equipment");
        Console.WriteLine("5. Rent equipment");
        Console.WriteLine("6. Return equipment");
        Console.WriteLine("7. Mark equipment as damaged");
        Console.WriteLine("8. Show rentals for user");
        Console.WriteLine("9. Show overdue rentals");
        Console.WriteLine("10. Show active rentals");
        Console.WriteLine("11. Show report");
        Console.WriteLine("0. Exit");
        Console.Write("Choose option: ");
    }

    private static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void AddUser()
    {
        Console.Write("First name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last name: ");
        var lastName = Console.ReadLine();
        Console.Write("Type (student/employee): ");
        var type = Console.ReadLine()?.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("First and last name cannot be empty.");
            Pause();
            return;
        }

        User? user = type switch
        {
            "student" => new Student(firstName, lastName),
            "employee" => new Employee(firstName, lastName),
            _ => null
        };

        if (user == null)
        {
            Console.WriteLine("Invalid type. Use: student / employee.");
            Pause();
            return;
        }

        service.AddUser(user);
        Console.WriteLine($"User {user.FirstName} {user.LastName} added with ID {user.Id}");
        Pause();
    }

    private static void AddEquipment()
    {
        Console.Write("Type (laptop/projector/camera): ");
        var type = Console.ReadLine()?.ToLowerInvariant();
        Console.Write("Name: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Equipment name cannot be empty.");
            Pause();
            return;
        }

        Equipment? eq = null;
        switch (type)
        {
            case "laptop":
                Console.Write("Processor: ");
                var proc = Console.ReadLine();
                Console.Write("RAM (GB): ");
                if (int.TryParse(Console.ReadLine(), out var ram))
                    eq = new Laptop(name, proc ?? string.Empty, ram);
                break;

            case "projector":
                Console.Write("Resolution: ");
                var res = Console.ReadLine();
                Console.Write("Lumens: ");
                if (int.TryParse(Console.ReadLine(), out var lum))
                    eq = new Projector(name, res ?? string.Empty, lum);
                break;

            case "camera":
                Console.Write("Megapixels: ");
                if (int.TryParse(Console.ReadLine(), out var mp))
                {
                    Console.Write("Lens type: ");
                    var lens = Console.ReadLine();
                    eq = new Camera(name, mp, lens ?? string.Empty);
                }
                break;
        }

        if (eq == null)
        {
            Console.WriteLine("Invalid type or invalid numeric input.");
            Pause();
            return;
        }

        service.AddEquipment(eq);
        Console.WriteLine($"Equipment {eq.Name} added with ID {eq.Id}");
        Pause();
    }

    private static void ListAllEquipment()
    {
        var all = service.GetAllEquipment();
        if (!all.Any())
        {
            Console.WriteLine("No equipment.");
        }
        else
        {
            foreach (var eq in all)
            {
                Console.WriteLine($"{eq.Id} - {eq.Name} (Available: {eq.IsAvailable}, Damaged: {eq.IsDamaged})");
            }
        }

        Pause();
    }

    private static void ListAvailableEquipment()
    {
        var available = service.GetAvailableEquipment();
        if (!available.Any())
        {
            Console.WriteLine("No available equipment.");
        }
        else
        {
            foreach (var eq in available)
            {
                Console.WriteLine($"{eq.Id} - {eq.Name}");
            }
        }

        Pause();
    }

    private static void RentEquipment()
    {
        var users = service.GetAllUsers();
        if (!users.Any())
        {
            Console.WriteLine("No users. Add a user first.");
            Pause();
            return;
        }

        Console.WriteLine("Users:");
        foreach (var u in users)
            Console.WriteLine($"{u.Id} - {u.FirstName} {u.LastName} ({u.GetType().Name})");

        Console.Write("Enter user ID: ");
        var userId = Console.ReadLine();

        var available = service.GetAvailableEquipment();
        if (!available.Any())
        {
            Console.WriteLine("No available equipment.");
            Pause();
            return;
        }

        Console.WriteLine("Available equipment:");
        foreach (var eq in available)
            Console.WriteLine($"{eq.Id} - {eq.Name}");

        Console.Write("Enter equipment ID: ");
        var eqId = Console.ReadLine();
        Console.Write("Rental duration (days): ");
        if (!int.TryParse(Console.ReadLine(), out int days) || days <= 0)
        {
            Console.WriteLine("Invalid days.");
            Pause();
            return;
        }

        var success = service.RentEquipment(userId ?? string.Empty, eqId ?? string.Empty, days);
        if (success)
            Console.WriteLine("Rental successful.");
        else
            Console.WriteLine("Rental failed. Check user limit, equipment availability, or IDs.");

        Pause();
    }

    private static void ReturnEquipment()
    {
        var activeRentals = service.GetActiveRentals();
        if (!activeRentals.Any())
        {
            Console.WriteLine("No active rentals.");
            Pause();
            return;
        }

        Console.WriteLine("Active rentals:");
        foreach (var rental in activeRentals)
        {
            Console.WriteLine(
                $"{rental.Id} - {rental.Equipment.Name} rented by {rental.User.FirstName} {rental.User.LastName}, due {rental.DueDate:yyyy-MM-dd}");
        }

        Console.Write("Enter rental ID to return: ");
        var rentalId = Console.ReadLine();

        var success = service.ReturnEquipment(rentalId ?? string.Empty);
        if (success)
            Console.WriteLine("Return processed. Fine (if any) will be shown.");
        else
            Console.WriteLine("Return failed. Invalid ID or already returned.");

        Pause();
    }

    private static void MarkEquipmentDamaged()
    {
        var all = service.GetAllEquipment();
        if (!all.Any())
        {
            Console.WriteLine("No equipment.");
            Pause();
            return;
        }

        foreach (var eq in all)
            Console.WriteLine($"{eq.Id} - {eq.Name} (Damaged: {eq.IsDamaged})");

        Console.Write("Enter equipment ID: ");
        var id = Console.ReadLine();

        if (service.MarkEquipmentAsDamaged(id ?? string.Empty))
            Console.WriteLine("Equipment marked as damaged.");
        else
            Console.WriteLine("Failed.");

        Pause();
    }

    private static void ShowUserRentals()
    {
        var users = service.GetAllUsers();
        if (!users.Any())
        {
            Console.WriteLine("No users.");
            Pause();
            return;
        }

        foreach (var u in users)
            Console.WriteLine($"{u.Id} - {u.FirstName} {u.LastName}");

        Console.Write("Enter user ID: ");
        var userId = Console.ReadLine();
        var rentals = service.GetRentalsForUser(userId ?? string.Empty);

        if (!rentals.Any())
        {
            Console.WriteLine("No rentals for this user.");
        }
        else
        {
            foreach (var r in rentals)
            {
                Console.WriteLine(
                    $"{r.Equipment.Name} - Rented: {r.RentDate:yyyy-MM-dd}, Due: {r.DueDate:yyyy-MM-dd}, Returned: {r.ReturnDate?.ToString("yyyy-MM-dd") ?? "not returned"}");
            }
        }

        Pause();
    }

    private static void ShowOverdueRentals()
    {
        var overdue = service.GetOverdueRentals();
        if (!overdue.Any())
        {
            Console.WriteLine("No overdue rentals.");
        }
        else
        {
            foreach (var r in overdue)
            {
                Console.WriteLine($"{r.Id} - {r.Equipment.Name} to {r.User.FirstName} {r.User.LastName}, due {r.DueDate:yyyy-MM-dd}");
            }
        }

        Pause();
    }

    private static void ShowActiveRentals()
    {
        var activeRentals = service.GetActiveRentals();
        if (!activeRentals.Any())
        {
            Console.WriteLine("No active rentals.");
        }
        else
        {
            foreach (var r in activeRentals)
            {
                Console.WriteLine(
                    $"{r.Id} - {r.Equipment.Name} rented by {r.User.FirstName} {r.User.LastName}, due {r.DueDate:yyyy-MM-dd}");
            }
        }

        Pause();
    }

    private static void ShowReport()
    {
        Console.WriteLine(service.GenerateReport());
        Pause();
    }
}
