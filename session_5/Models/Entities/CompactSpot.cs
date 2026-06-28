namespace Models.Entities;
using Models.Enums;
public class CompactSpot : ParkingSpot
{
    public CompactSpot()
    {
        Type = SpotType.Compact;
    }
    public override bool CanFitVehicle(VehicleType vehicle)
    {
        return vehicle == VehicleType.Car || vehicle == VehicleType.Motorcycle;
    }
}