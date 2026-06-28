namespace Models.Entities;
using Models.Enums;
public class Motorcycle : Vehicle
{
    public Motorcycle()
    {
        Type = VehicleType.Motorcycle;
    }
}