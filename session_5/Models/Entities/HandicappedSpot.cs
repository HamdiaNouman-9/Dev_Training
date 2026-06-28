namespace Models.Entities;
using Models.Enums;
public class HandicappedSpot : ParkingSpot
{
    public HandicappedSpot()
    {
        Type = SpotType.Handicapped;
    }
    public override bool CanFitVehicle(VehicleType vehicle)
    {
        return vehicle == VehicleType.Car || vehicle == VehicleType.Motorcycle;
    }
}