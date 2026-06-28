namespace Models.Entities;
using Models.Enums;
public class Car : Vehicle
{
    public Car()
    {
        Type = VehicleType.Car;
    }
}