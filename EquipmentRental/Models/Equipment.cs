namespace EquipmentRental.Models;

public abstract class Equipment
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool IsDamaged { get; set; } = false;

    protected Equipment(string name)
    {
        Id = Guid.NewGuid().ToString().Substring(0, 8);
        Name = name;
    }
}

