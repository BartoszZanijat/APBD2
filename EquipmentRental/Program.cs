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
            Console.WriteLine("=== Equipment Rental System ===");
            Console.WriteLine("1. Add user");
            Console.WriteLine("2. Add equipment");
            Console.WriteLine("3. List all equipment");
            Console.WriteLine("4. List available equipment");
            Console.WriteLine("5. Rent equipment");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

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
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
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
            Console.ReadKey();
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
            Console.ReadKey();
            return;
        }

        service.AddUser(user);
        Console.WriteLine($"User {user.FirstName} {user.LastName} added with ID {user.Id}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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
            Console.ReadKey();
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
            Console.ReadKey();
            return;
        }

        service.AddEquipment(eq);
        Console.WriteLine($"Equipment {eq.Name} added with ID {eq.Id}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
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

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void RentEquipment()
    {
        var users = service.GetAllUsers();
        if (!users.Any())
        {
            Console.WriteLine("No users. Add a user first.");
            Console.ReadKey();
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
            Console.ReadKey();
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
            Console.ReadKey();
            return;
        }

        var success = service.RentEquipment(userId ?? string.Empty, eqId ?? string.Empty, days);
        if (success)
            Console.WriteLine("Rental successful.");
        else
            Console.WriteLine("Rental failed. Check user limit, equipment availability, or IDs.");

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
