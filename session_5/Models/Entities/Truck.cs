namespace Models.Entities;
using Models.Enums;
public class Truck : Vehicle
{
    public Truck()
    {
        Type = VehicleType.Truck;
    }
}