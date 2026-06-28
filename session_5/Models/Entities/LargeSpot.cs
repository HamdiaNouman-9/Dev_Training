namespace Models.Entities;
using Models.Enums;
public class LargeSpot : ParkingSpot
{
    public LargeSpot()
    {
        Type = SpotType.Large;
    }
    public override bool CanFitVehicle(VehicleType vehicle)
    {
        return vehicle == VehicleType.Car || vehicle == VehicleType.Motorcycle || vehicle == VehicleType.Truck;
    }
}