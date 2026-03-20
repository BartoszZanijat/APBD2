namespace EquipmentRental.Models;

public class Camera : Equipment
{
    public int Megapixels { get; set; }
    public string LensType { get; set; }

    public Camera(string name, int megapixels, string lensType) : base(name)
    {
        Megapixels = megapixels;
        LensType = lensType;
    }
}

