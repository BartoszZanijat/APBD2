namespace EquipmentRental.Models;

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public int Lumens { get; set; }

    public Projector(string name, string resolution, int lumens) : base(name)
    {
        Resolution = resolution;
        Lumens = lumens;
    }
}

