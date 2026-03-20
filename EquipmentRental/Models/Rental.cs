namespace EquipmentRental.Models;

public class Rental
{
    public string Id { get; set; }
    public User User { get; set; }
    public Equipment Equipment { get; set; }
    public DateTime RentDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal? Fine { get; set; }

    public Rental(User user, Equipment equipment, DateTime rentDate, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString().Substring(0, 8);
        User = user;
        Equipment = equipment;
        RentDate = rentDate;
        DueDate = dueDate;
        ReturnDate = null;
        Fine = null;
    }
}

